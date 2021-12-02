using System.ComponentModel.DataAnnotations;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using Newtonsoft.Json;

namespace Tests.Model.Meeting
{
    [DisallowAdditionalProperties]
    public class ItemId
    {
        [JsonProperty("id", Required = Required.Always), Format("uuid")]
        public string Id { get; set; }

        [JsonProperty("change_key", Required = Required.Always), MinLength(1)]
        public string ChangeKey;
    }
}
