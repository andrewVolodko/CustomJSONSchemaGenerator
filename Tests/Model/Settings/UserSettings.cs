using System.ComponentModel.DataAnnotations;
using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace Tests.Model.Settings
{
    [GenerateJSONSchema]
    [AllowAdditionalProperties(false)]
    public class UserSettings : SettingsObj
    {
        [JsonProperty("email", Required = Required.Always), RegularExpression("^.+\\..+(@itechart-group\\.com)$")] 
        public string Email { get; set; }
        
        [JsonProperty("role", Required = Required.Always), EnumDataType(typeof(Role))] 
        public string Role { get; set; }
        

        public UserSettings(string settings, string email, string role) : base(settings)
        {
            Email = email;
            Role = role;
        }
    }
}
