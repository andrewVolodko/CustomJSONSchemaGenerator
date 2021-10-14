using System;
using System.Security.Principal;

namespace CustomJSONGenerator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public abstract class JSONSchemaTypeAttribute : Attribute { }

    public class MaximumPropertiesAttribute : JSONSchemaTypeAttribute
    {
        public int Value { get; }

        public MaximumPropertiesAttribute(int value) => Value = value;
    }

    public class MinimumPropertiesAttribute : JSONSchemaTypeAttribute
    {
        public int Value { get; }

        public MinimumPropertiesAttribute(int value) => Value = value;
    }
    
    public class AllowAdditionalPropertiesAttribute : JSONSchemaTypeAttribute
    {
        public bool Value { get; }

        public AllowAdditionalPropertiesAttribute(bool value = true) => Value = value;
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class GenerateJSONSchemaAttribute : Attribute {}
}