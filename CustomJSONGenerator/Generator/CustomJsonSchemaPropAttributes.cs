using System;
using System.ComponentModel;

namespace CustomJSONGenerator.Generator
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public abstract class JsonSchemaPropAttribute : Attribute {}
    
    public class MaximumAttribute : JsonSchemaPropAttribute
    {
        public double Value { get; }

        public MaximumAttribute(double value) => Value = value;
    }

    public class MinimumAttribute : JsonSchemaPropAttribute
    {
        public double Value { get; }

        public MinimumAttribute(double value) => Value = value;
    }

    [Description("If provided, Maximum property will be verified not inclusively")]
    public class ExclusiveMaximumAttribute : JsonSchemaPropAttribute {}

    [Description("If provided, Minimum property will be verified not inclusively")]
    public class ExclusiveMinimumAttribute : JsonSchemaPropAttribute {}

    public class MultipleOfAttribute : JsonSchemaPropAttribute
    {
        public double Value { get; }

        public MultipleOfAttribute(double value) => Value = value;
    }

    public class MaximumLengthAttribute : JsonSchemaPropAttribute
    {
        public ulong Value { get; }

        public MaximumLengthAttribute(ulong value) => Value = value;
    }

    public class MinimumLengthAttribute : JsonSchemaPropAttribute
    {
        public ulong Value { get; }

        public MinimumLengthAttribute(ulong value) => Value = value;
    }

    public class MinimumItemsAttribute : JsonSchemaPropAttribute
    {
        public ulong Value { get; }

        public MinimumItemsAttribute(ulong value) => Value = value;
    }

    public class MaximumItemsAttribute : JsonSchemaPropAttribute
    {
        public ulong Value { get; }

        public MaximumItemsAttribute(ulong value) => Value = value;
    }

    public class RegEx : JsonSchemaPropAttribute
    {
        public string Pattern { get; }

        public RegEx(string pattern) => Pattern = pattern;
    }

    public class DisallowAdditionalItemsAttribute : JsonSchemaPropAttribute {}

    public class StringFormat : JsonSchemaPropAttribute
    {
        public string Format { get; }

        public StringFormat(string format) => Format = format;
    }
}
