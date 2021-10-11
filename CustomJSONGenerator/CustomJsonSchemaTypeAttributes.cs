using System;

namespace CustomJSONGenerator
{
    public abstract class JSONSchemaTypeAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public class MaximumPropertiesAttribute : JSONSchemaTypeAttribute
    {
        public int Value { get; }

        public MaximumPropertiesAttribute(int value) => Value = value;
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class MinimumPropertiesAttribute : JSONSchemaTypeAttribute
    {
        public int Value { get; }

        public MinimumPropertiesAttribute(int value) => Value = value;
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class AllowAdditionalPropertiesAttribute : JSONSchemaTypeAttribute
    {
        public bool Value { get; }

        public AllowAdditionalPropertiesAttribute(bool value = true) => Value = value;
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class GenerateJSONSchemaAttribute : Attribute
    {
        public bool Value { get; }

        public GenerateJSONSchemaAttribute(bool value = true) => Value = value;
    }
}