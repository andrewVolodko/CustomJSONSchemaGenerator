using CustomJSONGenerator;
using Newtonsoft.Json;
using Tests.Model.Meeting;

namespace Tests.Model.Meetings
{
    [GenerateJSONSchema]
    [AllowAdditionalProperties(false)]
    public class MeetingAsync : BaseMeeting
    {
        [JsonProperty("attendees", Required = Required.Always), MinimumItems(2), AllowAdditionalItems(false)] 
        public NameEmailObj[] Attendees { get; set; }
        
        [JsonProperty("room", Required = Required.Always)] 
        public NameEmailObj Room { get; set; }
        
        [JsonProperty("start", Required = Required.Always), Minimum(0)] 
        public long Start { get; set; }
        
        [JsonProperty("end", Required = Required.Always), Minimum(0)] 
        public long End { get; set; }

        
        public MeetingAsync(ItemId itemId, string locationName, string subject, NameEmailObj[] attendees, NameEmailObj room, long start, long end) : base(itemId, locationName, subject)
        {
            Attendees = attendees;
            Room = room;
            Start = start;
            End = end;
        }
    }
}