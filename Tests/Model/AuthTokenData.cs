using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace Tests.Model
{
    [GenerateJSONSchema]
    [AllowAdditionalProperties(false)]
    public class AuthTokenData : IResponse

    {
    [JsonProperty("auth_token", Required = Required.Always), MinimumStringLength(1)]
    public string AuthToken { get; set; }

    [JsonProperty("issued", Required = Required.Always), Minimum(1)]
    public long Issued { get; set; }

    [JsonProperty("due", Required = Required.Always), Minimum(1)]
    public long Due { get; set; }
    }
}
