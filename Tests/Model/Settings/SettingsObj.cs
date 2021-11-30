using System.ComponentModel.DataAnnotations;
using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace Tests.Model.Settings
{
    [GenerateJsonSchema]
    public class SettingsObj : IResponse
    {
        [JsonProperty("settings", Required = Required.Always), MinLength(0), MaxLength(1000)]
        public string Settings { get; set; }
    }
}