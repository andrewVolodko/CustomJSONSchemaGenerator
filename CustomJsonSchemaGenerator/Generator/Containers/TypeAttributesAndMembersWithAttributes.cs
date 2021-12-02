using System;
using System.Collections.Generic;

namespace CustomJsonSchemaGenerator.Generator.Containers
{
    // Class stores Attributes of a type and all its props names with their attributes if presented
    internal class TypeAttributesAndMembersWithAttributes
    {
        internal List<Attribute> Attributes;
        internal Dictionary<string, List<Attribute>> MembersNamesWithAttributes;

        internal TypeAttributesAndMembersWithAttributes AddTypeAttributes(
            IEnumerable<Attribute> typeAttributes)
        {
            Attributes = new List<Attribute>(typeAttributes);
            return this;
        }

        internal TypeAttributesAndMembersWithAttributes AddMemberWithAttributes(string memberName,
            List<Attribute> attributes)
        {
            MembersNamesWithAttributes ??= new Dictionary<string, List<Attribute>>();
            MembersNamesWithAttributes.Add(memberName, attributes);

            return this;
        }
    }
}