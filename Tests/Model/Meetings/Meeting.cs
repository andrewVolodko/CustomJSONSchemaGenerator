using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Tests.Model.Meeting;

namespace Tests.Model.Meetings
{
    [GenerateJsonSchema]
    // [DisallowAdditionalProperties]
    [MaximumProperties(9)]
    [MinimumProperties(1)]
    [AdditionalProperties(JSchemaType.Integer, JSchemaType.String)]
    public class Meeting : BaseMeeting
    {
        [JsonProperty("attendees", Required = Required.AllowNull), ArrayItemsCannotBeNull, UniqueItems, Contains(JSchemaType.Object), MinContains(2), MinLength(2), MaxLength(100)]
        public List<NameEmailObj> Attendees { get; set; }
        
        [JsonProperty("room", Required = Required.AllowNull)]
        public NameEmailObj Room { get; set; }
        
        [JsonProperty("start"), Range(0, long.MaxValue), ExclusiveMinimum]
        public long Start { get; set; }
        
        [JsonProperty("end"), MultipleOf(10), Range(1, 150), ExclusiveMaximum, ExclusiveMinimum]
        public long End { get; set; }
    }
}