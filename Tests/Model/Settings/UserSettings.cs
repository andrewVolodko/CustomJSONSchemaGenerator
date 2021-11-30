using System.ComponentModel.DataAnnotations;
using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace Tests.Model.Settings
{
    [GenerateJsonSchema]
    [DisallowAdditionalProperties]
    public class UserSettings : SettingsObj
    {
        [JsonProperty("email", Required = Required.Always)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^.+\\..+(@itechart-group\\.com)$")]
        public string Email { get; set; }
        
        [JsonProperty("role", Required = Required.Always)]
        [EnumDataType(typeof(Role))]
        public string Role { get; set; }
    }
}
