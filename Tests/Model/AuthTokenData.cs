using System.ComponentModel.DataAnnotations;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using Newtonsoft.Json;

namespace Tests.Model
{
    [DisallowAdditionalProperties]
    public class AuthTokenData : IResponse
    {
        [JsonProperty("auth_token", Required = Required.AllowNull), MinLength(10), Format("uuid")]
        public string AuthToken;

        [JsonProperty("issued", Required = Required.Always)]
        public long Issued { get; set; }

        [JsonProperty("due", Required = Required.Always), Range(0, 3), ExclusiveMaximum]
        public long Due { get; set; }
    }
}