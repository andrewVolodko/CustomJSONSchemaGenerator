using System.ComponentModel.DataAnnotations;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using Newtonsoft.Json;

namespace Tests.Model.Meeting
{
    [DisallowAdditionalProperties]
    public class NameEmailObj
    {
        [JsonProperty("name", Required = Required.AllowNull), MinLength(1), MaxLength(100), Format("NameFormat")]
        public string Name { get; set; }
        
        [JsonProperty("email", Required = Required.Always)]
        [Format("email")]
        [RegularExpression("^.+\\..+(@itechart-group\\.com)$")]
        [MaxLength(250)]
        public string Email { get; set; }
    }
}