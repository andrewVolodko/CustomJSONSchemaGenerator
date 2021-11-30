using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace Tests.Model.Settings
{
    [GenerateJsonSchema]
    public class SettingsObj : IResponse

    {
    [JsonProperty("settings", Required = Required.Always)]
    public string Settings { get; set; }


    public SettingsObj(string settings) => Settings = settings;
    }
}
