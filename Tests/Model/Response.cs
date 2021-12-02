using System.ComponentModel.DataAnnotations;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using Newtonsoft.Json;

namespace Tests.Model
{
    [GenerateJsonSchema]
    [DisallowAdditionalProperties]
    public class Response<T> where T: IResponse, new()
    {
        [JsonProperty("result"), DisallowAdditionalItems]
        public T Result { get; set; }
    }
}