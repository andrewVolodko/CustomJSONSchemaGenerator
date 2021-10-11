using System;

namespace CustomJSONGenerator
{
    public abstract class JSONSchemaPropAttribute : Attribute {}
    
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class MaximumAttribute : JSONSchemaPropAttribute
    {
        public int Value { get; }

        public MaximumAttribute(int value) => Value = value;
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class MinimumAttribute : JSONSchemaPropAttribute
    {
        public int Value { get; }

        public MinimumAttribute(int value) => Value = value;
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class MaximumStringLengthAttribute : JSONSchemaPropAttribute
    {
        public uint Value { get; }

        public MaximumStringLengthAttribute(uint value) => Value = value;
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class MinimumStringLengthAttribute : JSONSchemaPropAttribute
    {
        public uint Value { get; }

        public MinimumStringLengthAttribute(uint value) => Value = value;
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class MinimumItemsAttribute : JSONSchemaPropAttribute
    {
        public uint Value { get; }

        public MinimumItemsAttribute(uint value) => Value = value;
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class MaximumItemsAttribute : JSONSchemaPropAttribute
    {
        public uint Value { get; }

        public MaximumItemsAttribute(uint value) => Value = value;
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class AllowAdditionalItemsAttribute : JSONSchemaPropAttribute
    {
        public bool Value { get; }

        public AllowAdditionalItemsAttribute(bool value = true) => Value = value;
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class StringFormat : JSONSchemaPropAttribute
    {
        public string Value { get; }

        public StringFormat(string value) => Value = value;
    }
}
