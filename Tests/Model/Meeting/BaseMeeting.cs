using System.ComponentModel.DataAnnotations;
using CustomJSONGenerator.Generator;
using Newtonsoft.Json;
using Required = Newtonsoft.Json.Required;

namespace Tests.Model.Meeting
{
    public abstract class BaseMeeting : IResponse
    {
        [JsonProperty("item_id")]
        public ItemId ItemId { get; set; }
        
        [JsonProperty("location_name", Required = Required.AllowNull)]
        public string LocationName { get; set; }
        
        [JsonProperty("subject", Required = Required.Always)]
        public string Subject { get; set; }
    }
}