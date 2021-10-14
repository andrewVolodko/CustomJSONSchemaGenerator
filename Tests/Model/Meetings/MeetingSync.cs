using CustomJSONGenerator;
using Newtonsoft.Json;
using Tests.Model.Meeting;

namespace Tests.Model.Meetings
{
    [GenerateJSONSchema]
    [AllowAdditionalProperties(false)]
    public class MeetingSync : MeetingAsync
    {
        [JsonProperty("event_id", Required = Required.Always), MinimumStringLength(1)] 
        public string EventId { get; set; }

        
        public MeetingSync(ItemId itemId, string locationName, string subject, NameEmailObj[] attendees, NameEmailObj room, long start, long end, string eventId)
            : base(itemId, locationName, subject, attendees, room, start, end) => EventId = eventId;
    }
}