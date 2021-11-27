using System.ComponentModel.DataAnnotations;
using CustomJSONGenerator.Generator;
using Newtonsoft.Json;
using Required = Newtonsoft.Json.Required;

namespace CustomJSONGenerator.Tests.Models
{
    public class SimpleStringFields
    {
        [GenerateJsonSchema]
        public class SimpleStringWithMinimumLengthRestriction
        {
            [JsonProperty("simpleString"), MinimumLength(1)]
            public string SimpleString { get; set; }
        }

        [GenerateJsonSchema]
        public class SimpleStringWithMaximumLengthRestriction
        {
            [MaximumLength(long.MaxValue)]
            public string SimpleString;
        }

        [GenerateJsonSchema]
        public class SimpleStringWithRegExRestriction
        {
            [RegEx("^.+(@mail\\.ru)$)")]
            public string SimpleString;
        }

        [GenerateJsonSchema]
        public class SimpleStringWithFormatRestriction
        {
            [StringFormat("uuid")]
            public string SimpleString;
        }
    }
}