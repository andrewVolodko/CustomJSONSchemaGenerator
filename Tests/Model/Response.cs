using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using Newtonsoft.Json;

namespace Tests.Model
{
    [GenerateJsonSchema]
    [DisallowAdditionalProperties]
    public class Response<T> where T: IResponse, new()
    {
        [JsonProperty("result", Required = Required.AllowNull), DisallowAdditionalItems]
        public T Result { get; set; }
    }
}