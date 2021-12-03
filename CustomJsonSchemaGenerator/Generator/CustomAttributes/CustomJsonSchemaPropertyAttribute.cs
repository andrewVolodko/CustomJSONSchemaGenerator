using System;
using System.ComponentModel;

namespace CustomJsonSchemaGenerator.Generator.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public abstract class CustomJsonSchemaPropertyAttribute : Attribute {}

    [Description("If provided, Maximum property will be verified not inclusively")]
    public class ExclusiveMaximumAttribute : CustomJsonSchemaPropertyAttribute {}

    [Description("If provided, Minimum property will be verified not inclusively")]
    public class ExclusiveMinimumAttribute : CustomJsonSchemaPropertyAttribute {}

    public class MultipleOfAttribute : CustomJsonSchemaPropertyAttribute
    {
        public double Value { get; }

        public MultipleOfAttribute(double value) => Value = value;
    }

    public class FormatAttribute : CustomJsonSchemaPropertyAttribute
    {
        public string Value { get; }

        public FormatAttribute(string value) => Value = value;
    }

    public class DisallowAdditionalItemsAttribute : CustomJsonSchemaPropertyAttribute {}

    [Description("Applicable only for enumerable types")]
    public class ArrayItemsCannotBeNullAttribute : CustomJsonSchemaPropertyAttribute {}
}
