using System.ComponentModel.DataAnnotations;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;

namespace CustomJsonSchemaGenerator.Tests.Models.SimpleObjects
{
    public class SimpleNumber
    {
        public class SimpleNumberWithExclusiveMinimum
        {
            [Range(double.MinValue, double.MaxValue), ExclusiveMinimum]
            public double SimpleNumberField;

            [Range(double.MinValue, double.MaxValue), ExclusiveMinimum]
            public double SimpleNumberProperty { get; set; }
        }

        public class SimpleNumberWithExclusiveMaximum
        {
            [Range(double.MinValue, double.MaxValue), ExclusiveMaximum]
            public double SimpleNumberField;

            [Range(double.MinValue, double.MaxValue), ExclusiveMaximum]
            public double SimpleNumberProperty { get; set; }
        }

        public class SimpleNumberWithExclusiveMinimumAndExclusiveMaximum
        {
            [Range(double.MinValue, double.MaxValue), ExclusiveMinimum, ExclusiveMaximum]
            public double SimpleNumberField;

            [Range(double.MinValue, double.MaxValue), ExclusiveMinimum, ExclusiveMaximum]
            public double SimpleNumberProperty { get; set; }
        }

        public class SimpleNumberWithMultipleOf
        {
            [MultipleOf(10)]
            public double SimpleNumberField;

            [MultipleOf(10)]
            public double SimpleNumberProperty { get; set; }
        }
    }
}