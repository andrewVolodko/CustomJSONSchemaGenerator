using CustomJSONGenerator.Generator;
using Newtonsoft.Json;
using Tests.Model.Meeting;

namespace Tests.Model.Meetings
{
    [GenerateJsonSchema]
    [DisallowAdditionalProperties]
    public class Meeting : BaseMeeting
    {
        [JsonProperty("attendees", Required = Required.AllowNull), MinimumItems(2), DisallowAdditionalItems]
        public NameEmailObj[] Attendees { get; set; }
        
        [JsonProperty("room", Required = Required.Always)] 
        public NameEmailObj[] Room { get; set; }
        
        [JsonProperty("start", Required = Required.Always), Minimum(0), Maximum(long.MaxValue)]
        public long Start { get; set; }
        
        [JsonProperty("end", Required = Required.Always), Minimum(0), ExclusiveMinimum, MultipleOf(10)]
        public long End { get; set; }
    }
}