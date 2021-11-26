using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace Tests.Model
{
    [GenerateJSONSchema]
    public class Response<T> where T: IResponse
    {
        [JsonProperty("result", Required = Required.AllowNull), MinimumStringLength(1), AllowAdditionalItems(false)]
        public T Result { get; set; }
    }
}