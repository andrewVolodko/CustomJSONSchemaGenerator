using CustomJSONGenerator;
using Newtonsoft.Json;
using Tests.Model.Meeting;
using Tests.Model.Meetings;

namespace Tests.Model
{
    [GenerateJSONSchema]
    public class Response<T> where T: IResponse
    {
        [JsonProperty("result", Required = Required.AllowNull), MinimumStringLength(1), AllowAdditionalItems(false)]
        public T Result { get; set; }
    }
}