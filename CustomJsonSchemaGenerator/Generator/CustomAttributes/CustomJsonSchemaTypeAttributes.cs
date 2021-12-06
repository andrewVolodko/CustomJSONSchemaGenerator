using System;
using System.Linq;
using Newtonsoft.Json.Schema;

namespace CustomJsonSchemaGenerator.Generator.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class ,Inherited = false)]
    public abstract class CustomJsonSchemaTypeAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class GenerateJsonSchemaAttribute : Attribute {}

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

    public class AdditionalPropertiesAttribute : CustomJsonSchemaTypeAttribute
    {
        private const string ContainsTypeString = "{{ \"type\": {0} }}";
        public JSchema Value { get; }

        public AdditionalPropertiesAttribute(params JSchemaType[] values)
        {
            var type = values
                .Aggregate("", (current, value) => current + $"\"{value.ToString().ToLower()}\" ")
                .TrimEnd()
                .Replace(" ", ", ");

            Value = JSchema.Parse(string.Format(ContainsTypeString, $"[{type}]"));
        }
    }
}