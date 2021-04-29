using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ZoomNet.Models.Webhooks
{
	/// <summary>
	/// Enumeration to indicate the type of webhook event.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum EventType
	{
		/// <summary>
		/// A user has uninstalled or deauthorized your app.
		/// </summary>
		[EnumMember(Value = "app_deauthorized")]
		AppDeauthorized,

		/// <summary>
		/// A service issue has been encountered during a meeting.
		/// </summary>
		[EnumMember(Value = "meeting.alerts")]
		MeetingServiceIssue,

		/// <summary>
		/// A meeting has been created.
		/// </summary>
		[EnumMember(Value = "meeting.created")]
		MeetingCreated,

		/// <summary>
		/// A meeting has been deleted.
		/// </summary>
		[EnumMember(Value = "meeting.deleted")]
		MeetingDeleted,

		/// <summary>
		/// A meeting has been updated.
		/// </summary>
		[EnumMember(Value = "meeting.updated")]
		MeetingUpdated,

		/// <summary>
		/// A meeting has been permanently deleted.
		/// </summary>
		[EnumMember(Value = "meeting.permanently_deleted")]
		MeetingPermanentlyDeleted,

		/// <summary>
		/// A meeting has started.
		/// </summary>
		[EnumMember(Value = "meeting.started")]
		MeetingStarted,

		/// <summary>
		/// A meeting has ended.
		/// </summary>
		[EnumMember(Value = "meeting.ended")]
		MeetingEnded,

		/// <summary>
		/// A meeting has been recovered.
		/// </summary>
		[EnumMember(Value = "meeting.recovered")]
		MeetingRecovered,

		/// <summary>
		/// A participant has registered for a meeting.
		/// </summary>
		[EnumMember(Value = "meeting.registration_created")]
		MeetingRegistrationCreated,

		/// <summary>
		/// A meeting registration has been approved.
		/// </summary>
		[EnumMember(Value = "meeting.registration_approved")]
		MeetingRegistrationApproved,

		/// <summary>
		/// A meeting registration has been cancelled.
		/// </summary>
		[EnumMember(Value = "meeting.registration_cancelled")]
		MeetingRegistrationCancelled,

		/// <summary>
		/// A meeting registration has been denied.
		/// </summary>
		[EnumMember(Value = "meeting.registration_denied")]
		MeetingRegistrationDenied,

		/// <summary>
		/// An attendee or the host has started sharing their screen.
		/// </summary>
		[EnumMember(Value = "meeting.sharing_started")]
		MeetingSharingStarted,

		/// <summary>
		/// An attendee or the host has stoped sharing their screen.
		/// </summary>
		[EnumMember(Value = "meeting.sharing_ended")]
		MeetingSharingEnded,

		/// <summary>
		/// An attendee is wating for the host to join the meeting.
		/// </summary>
		[EnumMember(Value = "meeting.participant_jbh_waiting")]
		MeetingParticipantWaitingForHost,

		/// <summary>
		/// An attendee has joined the meeting before the host.
		/// </summary>
		[EnumMember(Value = "meeting.participant_jbh_joined")]
		MeetingParticipantJoinedBeforeHost,

		/// <summary>
		/// An attendee has joined a meting waiting room.
		/// </summary>
		[EnumMember(Value = "meeting.participant_joined_waiting_room")]
		MeetingParticipantJoinedWaitingRoom,

		/// <summary>
		/// An attendee has left a meting waiting room.
		/// </summary>
		[EnumMember(Value = "meeting.participant_left_waiting_room")]
		MeetingParticipantLeftWaitingRoom,

		/// <summary>
		/// A meeting host has admitted a participant from a waiting room to the meeting.
		/// </summary>
		[EnumMember(Value = "meeting.participant_admitted")]
		MeetingParticipantAdmitted,

		/// <summary>
		/// A meeting host has admitted a participant from a waiting room to the meeting.
		/// </summary>
		[EnumMember(Value = "meeting.participant_joined")]
		MeetingParticipantJoined,

		/// <summary>
		/// A meeting participant who has already joined a meeting is sent back to the waiting room during the meeting.
		/// </summary>
		[EnumMember(Value = "meeting.participant_put_in_waiting_room")]
		MeetingParticipantSentToWaitingRoom,

		/// <summary>
		/// A meeting participant has left the meeting.
		/// </summary>
		[EnumMember(Value = "meeting.participant_left")]
		MeetingParticipantLeft,

		/// <summary>
		/// A meeting live stream has started.
		/// </summary>
		[EnumMember(Value = "meeting.live_streaming_started")]
		MeetingLiveStreamStarted,

		/// <summary>
		/// A meeting live stream has stoipped.
		/// </summary>
		[EnumMember(Value = "meeting.live_streaming_stopped")]
		MeetingLiveStreamStopped,

		/// <summary>
		/// A webinar has been created.
		/// </summary>
		[EnumMember(Value = "webinar.created")]
		WebinarCreated,

		/// <summary>
		/// A webinar has been deleted.
		/// </summary>
		[EnumMember(Value = "webinar.deleted")]
		WebinarDeleted,

		/// <summary>
		/// A webinar has been updated.
		/// </summary>
		[EnumMember(Value = "webinar.updated")]
		WebinarUpdated,

		/// <summary>
		/// A webinar has started.
		/// </summary>
		[EnumMember(Value = "webinar.started")]
		WebinarStarted,

		/// <summary>
		/// A webinar has ended.
		/// </summary>
		[EnumMember(Value = "webinar.ended")]
		WebinarEnded,
	}
}
