using CustomJSONGenerator.Generator;
using Newtonsoft.Json;
using Tests.Model.Meeting;

namespace Tests.Model.Meetings
{
    [GenerateJsonSchema]
    [DisallowAdditionalProperties]
    public class Meeting : BaseMeeting
    {
        [JsonProperty("attendees", Required = Required.AllowNull), DisallowAdditionalItems]
        public NameEmailObj[] Attendees { get; set; }
        
        [JsonProperty("room", Required = Required.Always)] 
        public NameEmailObj[] Room { get; set; }
        
        [JsonProperty("start", Required = Required.Always)]
        public long Start { get; set; }
        
        [JsonProperty("end", Required = Required.Always), MultipleOf(10)]
        public long End { get; set; }
    }
}