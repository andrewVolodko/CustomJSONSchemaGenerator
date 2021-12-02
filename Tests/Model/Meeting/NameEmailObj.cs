using System.ComponentModel.DataAnnotations;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using Newtonsoft.Json;

namespace Tests.Model.Meeting
{
    [DisallowAdditionalProperties]
    [CannotBeNullInArray]
    public class NameEmailObj
    {
        [JsonProperty("name", Required = Required.AllowNull), MinLength(1), MaxLength(100), Format("NameFormat")]
        public string Name { get; set; }
        
        [JsonProperty("email", Required = Required.Always)]
        [Format("email")]
        [RegularExpression("^.+\\..+(@itechart-group\\.com)$")]
        [MaxLength(250)]
        public string Email { get; set; }

        [JsonProperty("exchangeClient", Required = Required.Always)]
        public ExchangeClient ExchangeClient { get; set; }
    }

    public class ExchangeClient
    {
        [JsonProperty("version")]
        [Range(0, 100), ExclusiveMinimum]
        public double Version { get; set; }

        [Format("DescriptionFormat")]
        public string Description;
    }
}