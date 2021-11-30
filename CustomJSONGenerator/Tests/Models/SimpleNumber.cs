using System.ComponentModel.DataAnnotations;
using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace CustomJSONGenerator.Tests.Models
{
    public class SimpleNumber
    {
        // [GenerateJsonSchema]
        // public class SimpleNumberWithJsonPropertyName
        // {
        //     [JsonProperty("simpleNumberName")]
        //     public double SimpleNumber;
        // }
        //
        // [GenerateJsonSchema]
        // public class SimpleNumberWithRequiredAlways
        // {
        //     [JsonProperty(Required = Required.Always)]
        //     public double SimpleNumber;
        // }
        //
        // [GenerateJsonSchema]
        // public class SimpleNumberWithRequiredDefault
        // {
        //     [JsonProperty(Required = Required.Default)]
        //     public double SimpleNumber;
        // }
        //
        // [GenerateJsonSchema]
        // public class SimpleNumbersWithMinimum
        // {
        //     [Minimum(float.MinValue)]
        //     public float SimpleNumberFloat;
        //     [Minimum(double.MinValue)]
        //     public double SimpleNumberDouble;
        // }
        //
        [GenerateJsonSchema]
        public class SimpleNumbersWithExclusiveMinimum
        {
            [Range(double.MinValue, double.MaxValue), ExclusiveMinimum]
            public double SimpleNumberFloat;
        }
        //
        // [GenerateJsonSchema]
        // public class SimpleNumbersWithMaximum
        // {
        //     [Maximum(float.MaxValue)]
        //     public float SimpleNumberFloat;
        //     [Maximum(double.MaxValue)]
        //     public double SimpleNumberDouble;
        // }
        //
        // [GenerateJsonSchema]
        // public class SimpleNumbersWithExclusiveMaximum
        // {
        //     [Maximum(float.MaxValue), ExclusiveMaximum]
        //     public float SimpleNumber;
        //     [Maximum(double.MaxValue), ExclusiveMaximum]
        //     public double SimpleNumberDouble;
        // }
        //
        // [GenerateJsonSchema]
        // public class SimpleNumberWithMultipleOf
        // {
        //     [MultipleOf(10)]
        //     public double SimpleNumber;
        // }
    }
}