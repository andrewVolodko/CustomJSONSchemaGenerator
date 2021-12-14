using System.ComponentModel.DataAnnotations;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using Newtonsoft.Json;

namespace Tests.Model.Settings
{
    public class SettingsObj : IResponse
    {
        [JsonProperty("settings", Required = Required.Always), MinLength(0), MaxLength(1000)]
        public string Settings { get; set; }
    }
}