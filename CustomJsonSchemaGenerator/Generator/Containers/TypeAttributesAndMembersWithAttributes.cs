using System.Collections.Generic;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using Newtonsoft.Json;

namespace CustomJsonSchemaGenerator.Generator.Containers
{
    // Class stores Attributes of a type and all its props names with their attributes if presented
    internal class TypeAttributesAndMembersWithAttributes
    {
        internal List<JsonSchemaTypeAttribute> Attributes;
        internal Dictionary<string, List<JsonSchemaPropAttribute>> MembersNamesWithAttributes;
        internal Dictionary<string, Required> ArrayMembersNamesWithRequiredAttribute;

        internal TypeAttributesAndMembersWithAttributes AddTypeAttributes(
            IEnumerable<JsonSchemaTypeAttribute> typeAttributes)
        {
            Attributes = new List<JsonSchemaTypeAttribute>(typeAttributes);
            return this;
        }

        internal TypeAttributesAndMembersWithAttributes AddMemberWithAttributes(string memberName,
            List<JsonSchemaPropAttribute> attributes)
        {
            MembersNamesWithAttributes ??= new Dictionary<string, List<JsonSchemaPropAttribute>>();
            MembersNamesWithAttributes.Add(memberName, attributes);

            return this;
        }

        internal TypeAttributesAndMembersWithAttributes AddArrayMemberWithRequiredAttributes(string memberName,
            Required attribute)
        {
            ArrayMembersNamesWithRequiredAttribute ??= new Dictionary<string, Required>();
            ArrayMembersNamesWithRequiredAttribute.Add(memberName, attribute);

            return this;
        }
    }
}