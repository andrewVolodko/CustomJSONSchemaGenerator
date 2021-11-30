using System.ComponentModel.DataAnnotations;
using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace Tests.Model.Auth
{
    [GenerateJsonSchema]
    [DisallowAdditionalProperties]
    public class TokenData : AuthTokenData
    {
        [JsonProperty("refresh_token", Required = Required.Always)]
        public string RefreshToken { get; set; }
        
        [JsonProperty("session_id", Required = Required.Always), RegularExpression("[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}")]
        public string SessionId { get; set; }
        
        [JsonProperty("refresh_due", Required = Required.Always)]
        public long RefreshDue { get; set; }
    }
}
