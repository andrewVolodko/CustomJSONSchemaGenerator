using System.ComponentModel.DataAnnotations;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using Newtonsoft.Json;

namespace Tests.Model.Meeting
{
    [DisallowAdditionalProperties]
    public class NameEmailObj
    {
        [JsonProperty("name"), MinLength(1), MaxLength(100)]
        public string Name { get; set; }
        
        [JsonProperty("email")]
        [Format("email")]
        [RegularExpression("^.+\\..+(@itechart-group\\.com)$")]
        [MaxLength(250)]
        public string Email { get; set; }

        [JsonProperty("testfortestprop", Required = Required.AllowNull)]
        public TestForTest TestForTestProp { get; set; }
    }

    [DisallowAdditionalProperties]
    public class TestForTest
    {
        [JsonProperty("teststringprop")]
        [Format("SUPERSTRANGEFORMAT")]
        public string TestStringProp { get; set; }
    }
}