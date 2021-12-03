using System;

namespace CustomJsonSchemaGenerator.Generator.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class ,Inherited = false)]
    public abstract class CustomJsonSchemaTypeAttribute : Attribute { }

    public class MaximumPropertiesAttribute : CustomJsonSchemaTypeAttribute
    {
        public long Value { get; }

        public MaximumPropertiesAttribute(long value) => Value = value;
    }

    public class MinimumPropertiesAttribute : CustomJsonSchemaTypeAttribute
    {
        public long Value { get; }

        public MinimumPropertiesAttribute(long value) => Value = value;
    }
    
    public class DisallowAdditionalPropertiesAttribute : CustomJsonSchemaTypeAttribute {}


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class GenerateJsonSchemaAttribute : Attribute {}
}