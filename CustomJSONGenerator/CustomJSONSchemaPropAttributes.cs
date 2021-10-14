using System;

namespace CustomJSONGenerator
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, Inherited = false)]
    public abstract class JSONSchemaPropAttribute : Attribute {}
    
    public class MaximumAttribute : JSONSchemaPropAttribute
    {
        public int Value { get; }

        public MaximumAttribute(int value) => Value = value;
    }

    public class MinimumAttribute : JSONSchemaPropAttribute
    {
        public int Value { get; }

        public MinimumAttribute(int value) => Value = value;
    }

    public class MaximumStringLengthAttribute : JSONSchemaPropAttribute
    {
        public uint Value { get; }

        public MaximumStringLengthAttribute(uint value) => Value = value;
    }

    public class MinimumStringLengthAttribute : JSONSchemaPropAttribute
    {
        public uint Value { get; }

        public MinimumStringLengthAttribute(uint value) => Value = value;
    }

    public class MinimumItemsAttribute : JSONSchemaPropAttribute
    {
        public uint Value { get; }

        public MinimumItemsAttribute(uint value) => Value = value;
    }

    public class MaximumItemsAttribute : JSONSchemaPropAttribute
    {
        public uint Value { get; }

        public MaximumItemsAttribute(uint value) => Value = value;
    }

    public class AllowAdditionalItemsAttribute : JSONSchemaPropAttribute
    {
        public bool Value { get; }

        public AllowAdditionalItemsAttribute(bool value = true) => Value = value;
    }

    public class StringFormat : JSONSchemaPropAttribute
    {
        public string Value { get; }

        public StringFormat(string value) => Value = value;
    }
}
