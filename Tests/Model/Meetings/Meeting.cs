using System.Collections.Generic;
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
        [JsonProperty("attendees", NullValueHandling = NullValueHandling.Ignore), MinLength(2)]
        public List<NameEmailObj> Attendees { get; set; }
        
        [JsonProperty("room", NullValueHandling = NullValueHandling.Ignore)]
        public NameEmailObj Room { get; set; }
        
        [JsonProperty("start"), Range(0, long.MaxValue), ExclusiveMinimum]
        public long Start { get; set; }
        
        [JsonProperty("end"), MultipleOf(10), Range(1, 150), ExclusiveMaximum]
        public long End { get; set; }
    }
}