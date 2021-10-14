using CustomJSONGenerator;
using Newtonsoft.Json;

namespace Tests.Model.Settings
{
    [GenerateJSONSchema]
    public class SettingsObj

    {
    [JsonProperty("settings", Required = Required.Always), MinimumStringLength(1)]
    public string Settings { get; set; }


    public SettingsObj(string settings) => Settings = settings;
    }
}
