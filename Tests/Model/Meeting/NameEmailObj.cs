using System.ComponentModel.DataAnnotations;
using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace Tests.Model.Meeting
{
    [DisallowAdditionalProperties]
    public class NameEmailObj
    {
        [JsonProperty("name", Required = Required.Always), MinLength(1), MaxLength(100)]
        public string Name { get; set; }
        
        [JsonProperty("email", Required = Required.Always)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^.+\\..+(@itechart-group\\.com)$")]
        [MaxLength(250)]
        public string Email { get; set; }
    }
}