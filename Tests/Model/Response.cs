using System.ComponentModel.DataAnnotations;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using Newtonsoft.Json;

namespace Tests.Model
{
    [AllowAdditionalProperties]
    public class Response<T> where T: IResponse, new()
    {
        [JsonProperty("result", Required = Required.Always), MinLength(1)]
        public T Result { get; set; }
    }
}