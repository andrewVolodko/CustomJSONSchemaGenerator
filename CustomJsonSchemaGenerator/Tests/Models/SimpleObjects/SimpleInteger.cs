using System.ComponentModel.DataAnnotations;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;

namespace CustomJsonSchemaGenerator.Tests.Models.SimpleObjects
{
    public class SimpleInteger
    {
        public class SimpleIntegerWithExclusiveMinimum
        {
            [Range(int.MinValue, int.MaxValue), ExclusiveMinimum]
            public int SimpleIntegerField;

            [Range(int.MinValue, int.MaxValue), ExclusiveMinimum]
            public int SimpleIntegerProperty { get; set; }
        }

        public class SimpleIntegerWithExclusiveMaximum
        {
            [Range(int.MinValue, int.MaxValue), ExclusiveMaximum]
            public int SimpleIntegerField;

            [Range(int.MinValue, int.MaxValue), ExclusiveMaximum]
            public int SimpleIntegerProperty { get; set; }
        }

        public class SimpleIntegerWithExclusiveMinimumAndExclusiveMaximum
        {
            [Range(int.MinValue, int.MaxValue), ExclusiveMinimum, ExclusiveMaximum]
            public int SimpleIntegerField;

            [Range(int.MinValue, int.MaxValue), ExclusiveMinimum, ExclusiveMaximum]
            public int SimpleIntegerProperty { get; set; }
        }

        public class SimpleIntegerWithMultipleOf
        {
            [MultipleOf(10)]
            public int SimpleIntegerField;

            [MultipleOf(10)]
            public int SimpleIntegerProperty { get; set; }
        }
    }
}