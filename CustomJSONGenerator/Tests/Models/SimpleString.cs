using CustomJSONGenerator.Generator;

namespace CustomJSONGenerator.Tests.Models
{
    public class SimpleString
    {
        [GenerateJsonSchema]
        public class SimpleStringWithFormat
        {
            [Format("uuid")]
            public string SimpleString;
        }
    }
}