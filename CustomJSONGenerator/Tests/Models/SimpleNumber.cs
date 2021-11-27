using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace CustomJSONGenerator.Tests.Models
{
    public class SimpleNumber
    {
        [GenerateJsonSchema]
        public class SimpleNumberWithJsonPropertyName
        {
            [JsonProperty("simpleNumberName")]
            public double SimpleNumber;
        }

        [GenerateJsonSchema]
        public class SimpleNumberWithRequiredAlways
        {
            [JsonProperty(Required = Required.Always)]
            public double SimpleNumber;
        }

        [GenerateJsonSchema]
        public class SimpleNumberWithRequiredDefault
        {
            [JsonProperty(Required = Required.Default)]
            public double SimpleNumber;
        }

        [GenerateJsonSchema]
        public class SimpleNumberWithMinimum
        {
            [Minimum(double.MinValue)]
            public double SimpleNumber;
        }

        [GenerateJsonSchema]
        public class SimpleNumberWithExclusiveMinimum
        {
            [Minimum(double.MinValue), ExclusiveMinimum]
            public double SimpleNumber;
        }

        [GenerateJsonSchema]
        public class SimpleNumberWithMaximum
        {
            [Maximum(double.MaxValue)]
            public double SimpleNumber;
        }

        [GenerateJsonSchema]
        public class SimpleNumberWithExclusiveMaximum
        {
            [Maximum(double.MaxValue), ExclusiveMaximum]
            public double SimpleNumber;
        }

        [GenerateJsonSchema]
        public class SimpleNumberWithMultipleOf
        {
            [MultipleOf(10)]
            public double SimpleNumber;
        }
    }
}