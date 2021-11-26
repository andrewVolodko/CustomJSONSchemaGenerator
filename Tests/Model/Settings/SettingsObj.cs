using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace Tests.Model.Settings
{
    [GenerateJSONSchema]
    public class SettingsObj : IResponse

    {
    [JsonProperty("settings", Required = Required.Always), MinimumLength(1)]
    public string Settings { get; set; }


    public SettingsObj(string settings) => Settings = settings;
    }
}
