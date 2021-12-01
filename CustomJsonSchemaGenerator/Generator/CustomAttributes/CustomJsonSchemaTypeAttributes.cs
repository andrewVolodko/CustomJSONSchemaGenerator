using System;

namespace CustomJsonSchemaGenerator.Generator.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class ,Inherited = false)]
    public abstract class JsonSchemaTypeAttribute : Attribute { }

    public class MaximumPropertiesAttribute : JsonSchemaTypeAttribute
    {
        public long Value { get; }

        public MaximumPropertiesAttribute(long value) => Value = value;
    }

    public class MinimumPropertiesAttribute : JsonSchemaTypeAttribute
    {
        public long Value { get; }

        public MinimumPropertiesAttribute(long value) => Value = value;
    }
    
    public class DisallowAdditionalPropertiesAttribute : JsonSchemaTypeAttribute {}

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class GenerateJsonSchemaAttribute : Attribute {}
}