using System;
using System.ComponentModel;

namespace CustomJSONGenerator.Generator
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public abstract class JsonSchemaPropAttribute : Attribute {}

    [Description("If provided, Maximum property will be verified not inclusively")]
    public class ExclusiveMaximumAttribute : JsonSchemaPropAttribute {}

    [Description("If provided, Minimum property will be verified not inclusively")]
    public class ExclusiveMinimumAttribute : JsonSchemaPropAttribute {}

    public class MultipleOfAttribute : JsonSchemaPropAttribute
    {
        public double Value { get; }

        public MultipleOfAttribute(double value) => Value = value;
    }

    public class DisallowAdditionalItemsAttribute : JsonSchemaPropAttribute {}
}
