using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace Tests.Model
{
    [GenerateJsonSchema]
    [DisallowAdditionalProperties]
    public class Response<T> where T: IResponse
    {
        [JsonProperty("result", Required = Required.AllowNull), MinimumLength(1), DisallowAdditionalItems]
        public T Result { get; set; }
    }
}