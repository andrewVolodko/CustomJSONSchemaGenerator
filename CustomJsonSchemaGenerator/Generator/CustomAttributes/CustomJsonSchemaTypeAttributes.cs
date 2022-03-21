using System;
using System.Linq;
using Newtonsoft.Json.Schema;

namespace CustomJsonSchemaGenerator.Generator.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public abstract class CustomJsonSchemaTypeAttribute : Attribute { }

    public class MaximumPropertiesAttribute : CustomJsonSchemaTypeAttribute
    {
        public int Value { get; }

        public MaximumPropertiesAttribute(int value) => Value = value;
    }

    public class MinimumPropertiesAttribute : CustomJsonSchemaTypeAttribute
    {
        public int Value { get; }

        public MinimumPropertiesAttribute(int value) => Value = value;
    }

    public class AllowAdditionalPropertiesAttribute : CustomJsonSchemaTypeAttribute
    {
        public bool Value { get; }

        public AllowAdditionalPropertiesAttribute(bool value = true) => Value = value;
    }

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