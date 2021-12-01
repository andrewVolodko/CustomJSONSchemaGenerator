using System.ComponentModel.DataAnnotations;
using CustomJSONGenerator.Generator;

namespace CustomJSONGenerator.Tests.Models
{
    public class SimpleNumber
    {
        [GenerateJsonSchema]
        public class SimpleNumberWithExclusiveMinimum
        {
            [Range(double.MinValue, double.MaxValue), ExclusiveMinimum]
            public double SimpleNumberField;

            [Range(double.MinValue, double.MaxValue), ExclusiveMinimum]
            public double SimpleNumberProperty { get; set; }
        }

        [GenerateJsonSchema]
        public class SimpleNumberWithExclusiveMaximum
        {
            [Range(double.MinValue, double.MaxValue), ExclusiveMaximum]
            public double SimpleNumberField;

            [Range(double.MinValue, double.MaxValue), ExclusiveMaximum]
            public double SimpleNumberProperty { get; set; }
        }

        [GenerateJsonSchema]
        public class SimpleNumberWithExclusiveMinimumAndExclusiveMaximum
        {
            [Range(double.MinValue, double.MaxValue), ExclusiveMinimum, ExclusiveMaximum]
            public double SimpleNumberField;

            [Range(double.MinValue, double.MaxValue), ExclusiveMinimum, ExclusiveMaximum]
            public double SimpleNumberProperty { get; set; }
        }

        [GenerateJsonSchema]
        public class SimpleNumberWithMultipleOf
        {
            [MultipleOf(10)]
            public double SimpleNumberField;

            [MultipleOf(10)]
            public double SimpleNumberProperty { get; set; }
        }
    }
}