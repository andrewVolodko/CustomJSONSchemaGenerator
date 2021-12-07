using CustomJsonSchemaGenerator.Generator.CustomAttributes;

namespace CustomJsonSchemaGenerator.Tests.Models.SimpleObjects
{
    public class SimpleString
    {
        [GenerateJsonSchema]
        public class SimpleStringWithFormat
        {
            [Format("uuid")]
            public string SimpleStringField;

            [Format("uuid")]
            public string SimpleStringProperty { get; set; }
        }
    }
}