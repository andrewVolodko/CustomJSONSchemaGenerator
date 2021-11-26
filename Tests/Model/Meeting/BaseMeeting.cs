using System.ComponentModel.DataAnnotations;
using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace Tests.Model.Meeting
{
    public abstract class BaseMeeting : IResponse
    {
        [JsonProperty("item_id", Required = Required.Always)]
        public ItemId ItemId { get; set; }
        
        [JsonProperty("location_name", Required = Required.Always), MinimumLength(1)]
        public string LocationName { get; set; }
        
        [JsonProperty("subject", Required = Required.Always), MinimumLength(1)]
        public string Subject { get; set; }

        protected BaseMeeting(ItemId itemId, string locationName, string subject) : 
            this(locationName, subject) => ItemId = itemId;

        protected BaseMeeting(string locationName, string subject)
        {
            LocationName = locationName;
            Subject = subject;
        }
    }
}