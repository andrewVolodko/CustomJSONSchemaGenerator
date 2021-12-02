using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Tests.Model.Meeting
{
    public abstract class BaseMeeting : IResponse
    {
        [JsonProperty("item_id"), Required]
        public ItemId ItemId { get; set; }
        
        [JsonProperty("location_name", NullValueHandling = NullValueHandling.Include), Required, MinLength(20), MaxLength(250)]
        public string LocationName { get; set; }
        
        [JsonProperty("subject"), MinLength(1), MaxLength(250)]
        public string Subject { get; set; }
    }
}