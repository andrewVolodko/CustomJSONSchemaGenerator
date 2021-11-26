using System.ComponentModel.DataAnnotations;
using CustomJSONGenerator.Generator;
using Newtonsoft.Json;
using Tests.Model.Meeting;

namespace Tests.Model.Meetings
{
    [GenerateJSONSchema]
    [AllowAdditionalProperties(false)]
    public class Meeting : BaseMeeting
    {
        [JsonProperty("attendees", Required = Required.Always), MinimumItems(2), AllowAdditionalItems(false)] 
        public NameEmailObj[] Attendees { get; set; }
        
        [JsonProperty("room", Required = Required.Always)] 
        public NameEmailObj Room { get; set; }
        
        [JsonProperty("start", Required = Required.Always), Minimum(0), Maximum(200000)]
        public long Start { get; set; }
        
        [JsonProperty("end", Required = Required.Always), Minimum(0), ExclusiveMinimum, MultipleOf(10)]
        public long End { get; set; }

        
        public Meeting(ItemId itemId, string locationName, string subject, NameEmailObj[] attendees, NameEmailObj room, long start, long end) : base(itemId, locationName, subject)
        {
            Attendees = attendees;
            Room = room;
            Start = start;
            End = end;
        }
    }
}