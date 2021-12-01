using System.ComponentModel.DataAnnotations;
using CustomJSONGenerator.Generator;

namespace CustomJSONGenerator.Tests.Models
{
    public class SimpleInteger
    {
        [GenerateJsonSchema]
        public class SimpleIntegerWithExclusiveMinimum
        {
            [Range(int.MinValue, int.MaxValue), ExclusiveMinimum]
            public int SimpleIntegerField;

            [Range(int.MinValue, int.MaxValue), ExclusiveMinimum]
            public int SimpleIntegerProperty { get; set; }
        }

        [GenerateJsonSchema]
        public class SimpleIntegerWithExclusiveMaximum
        {
            [Range(int.MinValue, int.MaxValue), ExclusiveMaximum]
            public int SimpleIntegerField;

            [Range(int.MinValue, int.MaxValue), ExclusiveMaximum]
            public int SimpleIntegerProperty { get; set; }
        }

        [GenerateJsonSchema]
        public class SimpleIntegerWithExclusiveMinimumAndExclusiveMaximum
        {
            [Range(int.MinValue, int.MaxValue), ExclusiveMinimum, ExclusiveMaximum]
            public int SimpleIntegerField;

            [Range(int.MinValue, int.MaxValue), ExclusiveMinimum, ExclusiveMaximum]
            public int SimpleIntegerProperty { get; set; }
        }

        [GenerateJsonSchema]
        public class SimpleIntegerWithMultipleOf
        {
            [MultipleOf(10)]
            public int SimpleIntegerField;

            [MultipleOf(10)]
            public int SimpleIntegerProperty { get; set; }
        }
    }
}