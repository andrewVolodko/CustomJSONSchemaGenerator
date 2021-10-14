using System.ComponentModel.DataAnnotations;
using CustomJSONGenerator;
using Newtonsoft.Json;

namespace Tests.Model.Auth
{
    [GenerateJSONSchema]
    [AllowAdditionalProperties(false)]
    public class TokenData : AuthTokenData
    {
        [JsonProperty("refresh_token", Required = Required.Always), MinimumStringLength(1)] 
        public string RefreshToken { get; set; }
        
        [JsonProperty("session_id", Required = Required.Always), RegularExpression("[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}")]
        public string SessionId { get; set; }
        
        [JsonProperty("refresh_due", Required = Required.Always), Minimum(0)]
        public long RefreshDue { get; set; }
    }
}
