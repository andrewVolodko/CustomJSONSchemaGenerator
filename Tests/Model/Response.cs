using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace Tests.Model
{
    [GenerateJSONSchema]
    public class Response<T> where T: IResponse
    {
        [JsonProperty("result", Required = Required.AllowNull), MinimumLength(1), AllowAdditionalItems(false)]
        public T Result { get; set; }
    }
}