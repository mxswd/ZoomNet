using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;
using ZoomNet.Models.Webhooks;
using ZoomNet.Utilities;

namespace ZoomNet
{
	/// <summary>
	/// Client for Zoom's WebSocket webhooks.
	/// </summary>
	public class ZoomWebSocketClient : IDisposable
	{
		private readonly string _clientId;
		private readonly string _clientSecret;
		private readonly string _accountId;
		private readonly string _subscriptionId;
		private readonly ILogger _logger;
		private readonly IWebProxy _proxy;
		private readonly Action<Event> _eventProcessor;

		private WebsocketClient _websocketClient;
		private HttpClient _httpClient;
		private OAuthConnectionInfo _connectionInfo;
		private OAuthTokenHandler _tokenHandler;

		/// <summary>
		/// Initializes a new instance of the <see cref="ZoomWebSocketClient"/> class.
		/// </summary>
		/// <param name="clientId">Your Client Id.</param>
		/// <param name="clientSecret">Your Client Secret.</param>
		/// <param name="accountId">Your Account Id.</param>
		/// <param name="subscriptionId">Your subscirption Id.</param>
		/// <param name="eventProcessor">A delegate that will be invoked when a wehook message is received.</param>
		/// <param name="proxy">Allows you to specify a proxy.</param>
		/// <param name="logger">Logger.</param>
		public ZoomWebSocketClient(string clientId, string clientSecret, string accountId, string subscriptionId, Action<Event> eventProcessor, IWebProxy proxy = null, ILogger logger = null)
		{
			_clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
			_clientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
			_accountId = accountId ?? throw new ArgumentNullException(nameof(accountId));
			_subscriptionId = subscriptionId ?? throw new ArgumentNullException(nameof(subscriptionId));
			_eventProcessor = eventProcessor ?? throw new ArgumentNullException(nameof(eventProcessor));
			_proxy = proxy;
			_logger = logger ?? NullLogger.Instance;
		}

		/// <summary>
		/// Start listening to incoming webhooks from Zoom.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>Asynchronous task.</returns>
		public Task StartAsync(CancellationToken cancellationToken = default)
		{
			_connectionInfo = new OAuthConnectionInfo(_clientId, _clientSecret, _accountId, null);
			_httpClient = new HttpClient(new HttpClientHandler { Proxy = _proxy, UseProxy = _proxy != null });
			_tokenHandler = new OAuthTokenHandler(_connectionInfo, _httpClient);

			var clientFactory = new Func<Uri, CancellationToken, Task<WebSocket>>(async (uri, cancellationToken) =>
			{
				// The current value in the uri parameter must be ignored because it contains "access_token" which may have expired.
				// The following line ensures the "access_token" is refreshed whenever it expires.
				uri = new Uri($"wss://ws.zoom.us/ws?subscriptionId={_subscriptionId}&access_token={_tokenHandler.Token}");

				var client = new ClientWebSocket()
				{
					Options =
					{
						KeepAliveInterval = TimeSpan.Zero, // Turn off built-in "Keep Alive" feature because Zoom uses proprietary "heartbeat" every 30 seconds rather than standard "pong" messages at regular interval.
						Proxy = _proxy,
					}
				};
				client.Options.SetRequestHeader("ZoomNet-Version", ZoomClient.Version);

				await client.ConnectAsync(uri, cancellationToken).ConfigureAwait(false);
				return client;
			});

			_websocketClient = new WebsocketClient(new Uri("wss://ws.zoom.us"), clientFactory);
			_websocketClient.Name = "ZoomNet";
			_websocketClient.ReconnectTimeout = TimeSpan.FromSeconds(45); // Greater than 30 seconds because we send a heartbeat every 30 seconds
			_websocketClient.ErrorReconnectTimeout = TimeSpan.FromSeconds(45);
			_websocketClient.ReconnectionHappened.Subscribe(info => _logger.LogTrace($"Reconnection happened, type: {info.Type}"));
			_websocketClient.DisconnectionHappened.Subscribe(info => _logger.LogTrace($"Disconnection happened, type: {info.Type}"));
			_websocketClient.MessageReceived.Subscribe(ProcessMessage);

			Task.Run(() => SendHeartbeat(_websocketClient, cancellationToken));

			return _websocketClient.Start();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			// Call 'Dispose' to release resources
			Dispose(true);

			// Tell the GC that we have done the cleanup and there is nothing left for the Finalizer to do
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				ReleaseManagedResources();
			}
			else
			{
				// The object went out of scope and the Finalizer has been called.
				// The GC will take care of releasing managed resources, therefore there is nothing to do here.
			}

			ReleaseUnmanagedResources();
		}

		private async Task SendHeartbeat(IWebsocketClient client, CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				await Task.Delay(TimeSpan.FromSeconds(30)); // Zoom requires a heartbeat every 30 seconds

				if (!client.IsRunning)
				{
					_logger.LogTrace("Client is not running. Skipping heartbeat");
					continue;
				}

				_logger.LogTrace("Sending heartbeat");

				await client.SendInstant("ping").ConfigureAwait(false);
				client.Send("{\"module\":\"heartbeat\"}");
			}
		}

		private void ProcessMessage(ResponseMessage msg)
		{
			var jsonDoc = JsonDocument.Parse(msg.Text);
			var module = jsonDoc.RootElement.GetPropertyValue("module", string.Empty);
			var content = jsonDoc.RootElement.GetPropertyValue("content", string.Empty);

			switch (module)
			{
				case "build_connection":
					_logger.LogTrace("Received message: {module}. Connection has been established.", module);
					break;
				case "heartbeat":
					_logger.LogTrace("Received message: {module}. Server is acknowledging heartbeat.", module);
					break;
				case "message":
					var parser = new WebhookParser();
					var webhookEvent = parser.ParseEventWebhook(content);
					var eventType = webhookEvent.EventType;
					_logger.LogTrace("Received webhook event: {eventType}", eventType);
					try
					{
						_eventProcessor.Invoke(webhookEvent);
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "An error occurred while processing webhook event: {eventType}", eventType);
					}

					break;
				default:
					_logger.LogError("Received unknown message: {module}", module);
					break;
			}
		}

		private void ReleaseManagedResources()
		{
			_connectionInfo = null;
			_tokenHandler = null;

			if (_websocketClient != null)
			{
				_websocketClient.Dispose();
				_websocketClient = null;
			}

			if (_httpClient != null)
			{
				_httpClient.Dispose();
				_httpClient = null;
			}
		}

		private void ReleaseUnmanagedResources()
		{
			// We do not hold references to unmanaged resources
		}
	}
}
