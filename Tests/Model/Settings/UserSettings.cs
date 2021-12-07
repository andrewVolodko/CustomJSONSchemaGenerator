using System.ComponentModel.DataAnnotations;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using Newtonsoft.Json;

namespace Tests.Model.Settings
{
    [GenerateJsonSchema]
    [DisallowAdditionalProperties]
    public class UserSettings : SettingsObj
    {
        [JsonProperty("email", Required = Required.Always)]
        [RegularExpression("^.+\\..+(@itechart-group\\.com)$")]
        public string Email { get; set; }
        
        [JsonProperty("role", Required = Required.Always)]
        [EnumDataType(typeof(Role))]
        public string Role { get; set; }
    }
}
