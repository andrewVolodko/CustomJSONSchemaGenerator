using System;
using System.ComponentModel;

namespace CustomJSONGenerator.Generator
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, Inherited = false)]
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

    public class MaximumLengthAttribute : JsonSchemaPropAttribute
    {
        public ulong Value { get; }

        public MaximumLengthAttribute(ulong value) => Value = value;
    }

    public class MinimumStringLengthAttribute : JsonSchemaPropAttribute
    {
        public ulong Value { get; }

        public MinimumStringLengthAttribute(ulong value) => Value = value;
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

    public class AllowAdditionalItemsAttribute : JsonSchemaPropAttribute
    {
        public bool Value { get; }

        public AllowAdditionalItemsAttribute(bool value = true) => Value = value;
    }

    public class StringFormat : JsonSchemaPropAttribute
    {
        public string Value { get; }

        public StringFormat(string value) => Value = value;
    }
}
