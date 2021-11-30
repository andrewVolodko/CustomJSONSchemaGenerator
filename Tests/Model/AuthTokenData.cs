using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace Tests.Model
{
    [GenerateJsonSchema]
    [DisallowAdditionalProperties]
    public class AuthTokenData : IResponse

    {
    [JsonProperty("auth_token", Required = Required.Always)]
    public string AuthToken { get; set; }

    [JsonProperty("issued", Required = Required.Always)]
    public long Issued { get; set; }

    [JsonProperty("due", Required = Required.Always)]
    public long Due { get; set; }
    }
}
