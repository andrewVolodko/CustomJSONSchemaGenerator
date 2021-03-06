using System;
using System.ComponentModel;
using Newtonsoft.Json.Schema;

namespace CustomJsonSchemaGenerator.Generator.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public abstract class CustomJsonSchemaPropertyAttribute : Attribute {}

    [Description("If provided, Maximum property will be verified not inclusively")]
    public class ExclusiveMaximumAttribute : CustomJsonSchemaPropertyAttribute {}

    [Description("If provided, Minimum property will be verified not inclusively")]
    public class ExclusiveMinimumAttribute : CustomJsonSchemaPropertyAttribute {}

    public class MultipleOfAttribute : CustomJsonSchemaPropertyAttribute
    {
        public double Value { get; }

        public MultipleOfAttribute(double value) => Value = value;
    }

    public class FormatAttribute : CustomJsonSchemaPropertyAttribute
    {
        public string Value { get; }

        public FormatAttribute(string value) => Value = value;
    }

    public class DisallowAdditionalItemsAttribute : CustomJsonSchemaPropertyAttribute {}

    [Description("Applicable only for enumerable types")]
    public class ArrayItemsCannotBeNullAttribute : CustomJsonSchemaPropertyAttribute {}

    public class ContainsAttribute : CustomJsonSchemaPropertyAttribute
    {
        private const string ContainsTypeString = "{{ \"type\": \"{0}\" }}";
        public JSchema Value { get; }

        public ContainsAttribute(JSchemaType value) =>
            Value = JSchema.Parse(string.Format(ContainsTypeString, value.ToString().ToLower()));
    }

    [Description("Will be added to schema only if Contains attribute provided")]
    public class MaxContainsAttribute : CustomJsonSchemaPropertyAttribute
    {
        public long Value { get; }

        public MaxContainsAttribute(long value) => Value = value;
    }

    [Description("Will be added to schema only if Contains attribute provided")]
    public class MinContainsAttribute : CustomJsonSchemaPropertyAttribute
    {
        public long Value { get; }

        public MinContainsAttribute(long value) => Value = value;
    }

    public class UniqueItemsAttribute : CustomJsonSchemaPropertyAttribute { }
}
