using System;
using ZoomNet.Resources;

namespace ZoomNet
{
	/// <summary>
	/// Interface for the Zoom REST client.
	/// </summary>
	public interface IZoomClient
	{
		/// <summary>
		/// Gets the resource which allows you to manage sub accounts.
		/// </summary>
		IAccounts Accounts { get; }

		/// <summary>
		/// Gets the resource which allows you to manage call logs.
		/// </summary>
		ICallLogs CallLogs { get; }

		/// <summary>
		/// Gets the resource which allows you to manage chat channels, messages, etc.
		/// </summary>
		IChat Chat { get; }

		/// <summary>
		/// Gets the resource which allows you to manage chatbot messages.
		/// </summary>
		IChatbot Chatbot { get; }

		/// <summary>
		/// Gets the resource which allows you to manage cloud recordings.
		/// </summary>
		/// <value>
		/// The cloud recordings resource.
		/// </value>
		ICloudRecordings CloudRecordings { get; }

		/// <summary>
		/// Gets the resource which allows you to manage contacts.
		/// </summary>
		/// <value>
		/// The contacts resource.
		/// </value>
		IContacts Contacts { get; }

		/// <summary>
		/// Gets the resource which allows you to view metrics.
		/// </summary>
		IDashboards Dashboards { get; }

		/// <summary>
		/// Gets the resource which allows you to notify Zoom that you comply with the policy which requires
		/// you to handle user's data in accordance to the user's preference after the user uninstalls your app.
		/// </summary>
		[Obsolete("The Data Compliance API is deprecated")]
		IDataCompliance DataCompliance { get; }

		/// <summary>
		/// Gets the resource that allows you to manage groups.
		/// </summary>
		IGroups Groups { get; }

		/// <summary>
		/// Gets the resource which allows you to manage meetings.
		/// </summary>
		IMeetings Meetings { get; }

		/// <summary>
		/// Gets the resource which allows you to manage meetings that occured in the past.
		/// </summary>
		IPastMeetings PastMeetings { get; }

		/// <summary>
		/// Gets the resource which allows you to manage webinars that occured in the past.
		/// </summary>
		IPastWebinars PastWebinars { get; }

		/// <summary>
		/// Gets the resource which allows you to access Zoom Phone API endpoints.
		/// </summary>
		IPhone Phone { get; }

		/// <summary>
		/// Gets the resource which allows you to view reports.
		/// </summary>
		IReports Reports { get; }

		/// <summary>
		/// Gets the resource which allows you to manage roles.
		/// </summary>
		IRoles Roles { get; }

		/// <summary>
		/// Gets the resource which allows you to manage SMS messages and sessions.
		/// </summary>
		ISms Sms { get; }

		/// <summary>
		/// Gets the resource which allows you to manage users.
		/// </summary>
		IUsers Users { get; }

		/// <summary>
		/// Gets the resource which allows you to manage webinars.
		/// </summary>
		IWebinars Webinars { get; }
	}
}
