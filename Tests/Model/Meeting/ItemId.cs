using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace Tests.Model.Meeting
{
    [AllowAdditionalProperties(false)]
    public class ItemId
    {
        [JsonProperty("id", Required = Required.Always), MinimumLength(1)]
        public string Id { get; set; }

        [JsonProperty("change_key", Required = Required.Always), MinimumLength(1)]
        public string ChangeKey { get; set; }

        [JsonConstructor]
        public ItemId(string id, string changeKey)
        {
            Id = id;
            ChangeKey = changeKey;
        }
    }
}
