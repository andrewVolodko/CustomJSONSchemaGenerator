using System;
using System.ComponentModel;

namespace CustomJsonSchemaGenerator.Generator.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public abstract class CustomJsonSchemaPropAttribute : Attribute {}

    [Description("If provided, Maximum property will be verified not inclusively")]
    public class ExclusiveMaximumAttribute : CustomJsonSchemaPropAttribute {}

    [Description("If provided, Minimum property will be verified not inclusively")]
    public class ExclusiveMinimumAttribute : CustomJsonSchemaPropAttribute {}

    public class MultipleOfAttribute : CustomJsonSchemaPropAttribute
    {
        public double Value { get; }

        public MultipleOfAttribute(double value) => Value = value;
    }

    public class FormatAttribute : CustomJsonSchemaPropAttribute
    {
        public string Value { get; }

        public FormatAttribute(string value) => Value = value;
    }

    public class DisallowAdditionalItemsAttribute : CustomJsonSchemaPropAttribute {}

    [Description("Applicable only for enumerable types")]
    public class ArrayItemsCannotBeNullAttribute : CustomJsonSchemaPropAttribute {}
}
