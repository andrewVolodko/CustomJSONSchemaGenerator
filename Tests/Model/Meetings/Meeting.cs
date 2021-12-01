using System.ComponentModel.DataAnnotations;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using Newtonsoft.Json;
using Tests.Model.Meeting;

namespace Tests.Model.Meetings
{
    [GenerateJsonSchema]
    [DisallowAdditionalProperties]
    public class Meeting : BaseMeeting
    {
        [JsonProperty("attendees", Required = Required.AllowNull), MinLength(2)]
        public NameEmailObj[] Attendees { get; set; }
        
        [JsonProperty("room", Required = Required.Always)]
        public NameEmailObj Room { get; set; }
        
        [JsonProperty("start"), Required, Range(0, long.MaxValue), ExclusiveMinimum]
        public long Start { get; set; }
        
        [JsonProperty("end"), Required, MultipleOf(10), Range(1, 150), ExclusiveMaximum]
        public long End { get; set; }
    }
}