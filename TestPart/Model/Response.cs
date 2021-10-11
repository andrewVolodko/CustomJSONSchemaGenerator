using CustomJSONGenerator;
using Newtonsoft.Json;

namespace TestPart.Model
{
    public class Response<T> where T: class
    {
        [JsonProperty("result", Required = Required.AllowNull), MinimumStringLength(1), AllowAdditionalItems(false)]
        public T Result { get; set; }
    }
}