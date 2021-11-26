using System;

namespace CustomJSONGenerator.Generator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public abstract class JsonSchemaTypeAttribute : Attribute { }

    public class MaximumPropertiesAttribute : JsonSchemaTypeAttribute
    {
        public ulong Value { get; }

        public MaximumPropertiesAttribute(ulong value) => Value = value;
    }

    public class MinimumPropertiesAttribute : JsonSchemaTypeAttribute
    {
        public ulong Value { get; }

        public MinimumPropertiesAttribute(ulong value) => Value = value;
    }
    
    public class AllowAdditionalPropertiesAttribute : JsonSchemaTypeAttribute
    {
        public bool Value { get; }

        public AllowAdditionalPropertiesAttribute(bool value = true) => Value = value;
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class GenerateJSONSchemaAttribute : Attribute {}
}