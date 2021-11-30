using System.ComponentModel.DataAnnotations;
using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace Tests.Model.Meeting
{
    [DisallowAdditionalProperties]
    public class ItemId
    {
        [JsonProperty("id", Required = Required.Always), Format("uuid")]
        public string Id { get; set; }

        [JsonProperty("change_key", Required = Required.Always)]
        public string ChangeKey { get; set; }

        [JsonConstructor]
        public ItemId(string id, string changeKey)
        {
            Id = id;
            ChangeKey = changeKey;
        }
    }
}
