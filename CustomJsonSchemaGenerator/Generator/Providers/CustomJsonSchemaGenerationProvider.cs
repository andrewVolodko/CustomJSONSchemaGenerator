using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using CustomJsonSchemaGenerator.Generator.Containers;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace CustomJsonSchemaGenerator.Generator.Providers
{
    // Class to extend JSchemaGenerationProvider functionality. E.g. add more jsonSchema field and object attributes
    internal class CustomJsonSchemaGenerationProvider : JSchemaGenerationProvider
    {
        public override JSchema GetSchema(JSchemaTypeGenerationContext context)
        {
            // var typesWithAttributesAndMembersWithAttributes =
            //     new Dictionary<Type, TypeAttributesAndMembersWithAttributes>();
            //
            // GetCustomAttributesFromTypeAndItsMembers(context.ObjectType,
            //     ref typesWithAttributesAndMembersWithAttributes);
            //
            // foreach (var (type, typeAttributesAndMembersWithAttributes) in typesWithAttributesAndMembersWithAttributes)
            // {
            //     GenerateSchema(context, type, typeAttributesAndMembersWithAttributes);
            // }

            var schema = context.Generator.Generate(context.ObjectType);

            return context.Generator.Generate(context.ObjectType);
        }

        public override bool CanGenerateSchema(JSchemaTypeGenerationContext context) =>
            context.ObjectType.Namespace != "System";

        private static void GetCustomAttributesFromTypeAndItsMembers(Type baseType,
            ref Dictionary<Type, TypeAttributesAndMembersWithAttributes> typesWithAttributesAndItsMembersWithAttributes)
        {
            // Getting base type attributes. Adding to Dictionary if attrs were found
            var baseTypeCustomAttributes = GetMemberCustomAttributes(baseType);

            TypeAttributesAndMembersWithAttributes typeAttributesAndPropsWithAttributes;

            if (baseTypeCustomAttributes != null)
            {
                typeAttributesAndPropsWithAttributes =
                    new TypeAttributesAndMembersWithAttributes().AddTypeAttributes(baseTypeCustomAttributes);
                typesWithAttributesAndItsMembersWithAttributes.Add(baseType, typeAttributesAndPropsWithAttributes);
            }

            // Getting all properties of baseType
            var properties = baseType.GetProperties();

            // Loop through found properties
            LoopThroughFoundMembers(properties, ref typesWithAttributesAndItsMembersWithAttributes);

            // Getting all fields of baseType
            var fields = baseType.GetFields();

            // Loop through found fields
            LoopThroughFoundMembers(fields, ref typesWithAttributesAndItsMembersWithAttributes);

            void LoopThroughFoundMembers(IEnumerable<dynamic> members,
                ref Dictionary<Type, TypeAttributesAndMembersWithAttributes> typesWithItsMembersWithAttributes)
            {
                foreach (var member in members)
                {
                    // Getting name and attributes of the current member
                    var memberName = GetMemberName(member);
                    var memberCustomAttributes = GetMemberCustomAttributes(member);

                    if (memberCustomAttributes != null)
                    {
                        // Add property with attributes if they were found
                        try
                        {
                            typesWithItsMembersWithAttributes[baseType]
                                .AddMemberWithAttributes(memberName, memberCustomAttributes);
                        }
                        catch (KeyNotFoundException)
                        {
                            typeAttributesAndPropsWithAttributes = new TypeAttributesAndMembersWithAttributes()
                                .AddMemberWithAttributes(memberName, memberCustomAttributes);

                            typesWithItsMembersWithAttributes.Add(baseType,
                                typeAttributesAndPropsWithAttributes);
                        }
                    }

                    var curPropType = member is PropertyInfo ? member.PropertyType : member.FieldType;

                    if (curPropType.Namespace == "System" ||
                        typesWithItsMembersWithAttributes.ContainsKey(curPropType)) continue;

                    // Invoke method to get attributes of a property type if they presented
                    GetCustomAttributesFromTypeAndItsMembers(curPropType, ref typesWithItsMembersWithAttributes);
                }
            }
        }

        private static string GetMemberName(MemberInfo member)
        {
            var jsonPropertyAttributes = member.GetCustomAttributes(typeof(JsonPropertyAttribute), false);

            return jsonPropertyAttributes.Any()
                ? ((JsonPropertyAttribute) jsonPropertyAttributes.Single()).PropertyName ?? member.Name
                : member.Name;
        }

        // Getting CustomJsonSchemaType and Required attributes from type
        private static List<Attribute> GetMemberCustomAttributes(MemberInfo member)
        {
            var typeCustomAttributes = member.GetCustomAttributes(false)
                .Where(el => el.GetType() == typeof(CustomJsonSchemaTypeAttribute) ||
                             el.GetType() == typeof(JsonPropertyAttribute))
                .Select(el => (Attribute) el).ToList();

            return typeCustomAttributes.Count > 0 ? typeCustomAttributes : null;
        }

        private static JSchema _schema;

        private static void GenerateSchema(JSchemaTypeGenerationContext context,
            Type type, TypeAttributesAndMembersWithAttributes typeAttributesAndItsPropsWithAttributes)
        {
            _schema = context.Generator.Generate(type);

            AddCustomPropertiesToJsonTypesIfExist(typeAttributesAndItsPropsWithAttributes);

            AddCustomPropertiesToJsonPropertiesIfExist(typeAttributesAndItsPropsWithAttributes);
        }

        // This is just a "plug" in order to add ability of having nullable arrays or objects in schema
        // This feature is presented in Newtonsoft lib, but does not work for some reason
        private static JSchema AddNullTypeToPropertyIfRequired(JSchema propertySchema)
        {
            var regexEnding = propertySchema.Type switch
            {
                JSchemaType.Array => "(?=,\\n.*items)",
                JSchemaType.Object => "(?=,\\n.*[pP]roperties)",
                _ => null
            };

            var pattern = $"(?<=\"\\$id\"[\\S\\s]+,\\n.*type\": ).*{regexEnding}";

            var stringSchema = propertySchema.ToString();

            var matchValue = new Regex(pattern).Match(stringSchema).Value;

            var fixedStringSchema = Regex.Replace(stringSchema, pattern, $"[{matchValue}, \"null\"]");

            return JSchema.Parse(fixedStringSchema);
        }

        private static void AddCustomPropertiesToJsonTypesIfExist(
            TypeAttributesAndMembersWithAttributes typeAttributesAndItsPropsWithAttributes)
        {
            if (typeAttributesAndItsPropsWithAttributes.Attributes == null) return;

            // Iterate through type attributes
            foreach (var typeAttribute in typeAttributesAndItsPropsWithAttributes.Attributes)
            {
                switch (typeAttribute)
                {
                    case MaximumPropertiesAttribute maxProperties:
                        _schema.MaximumProperties = maxProperties.Value;
                        break;
                    case MinimumPropertiesAttribute minProperties:
                        _schema.MinimumProperties = minProperties.Value;
                        break;
                    case DisallowAdditionalPropertiesAttribute:
                        _schema.AllowAdditionalProperties = false;
                        break;
                }
            }
        }

        private static void AddCustomPropertiesToJsonPropertiesIfExist(
            TypeAttributesAndMembersWithAttributes typeAttributesAndItsPropsWithAttributes)
        {
            if (typeAttributesAndItsPropsWithAttributes.MembersNamesWithAttributes == null) return;

            foreach (var (propName, propAttributes) in
                typeAttributesAndItsPropsWithAttributes.MembersNamesWithAttributes)
            {
                foreach (var propAttribute in propAttributes)
                {
                    var currentProperty = _schema.Properties[propName];
                    var currentPropertyType = currentProperty.Type;

                    switch (currentPropertyType)
                    {
                        case JSchemaType.Number:
                        case JSchemaType.Integer:
                        case JSchemaType.Number | JSchemaType.Null:
                        case JSchemaType.Integer | JSchemaType.Null:
                            switch (propAttribute)
                            {
                                case ExclusiveMaximumAttribute:
                                    currentProperty.ExclusiveMaximum = true;
                                    break;
                                case ExclusiveMinimumAttribute:
                                    currentProperty.ExclusiveMinimum = true;
                                    break;
                                case MultipleOfAttribute multipleOfAttribute:
                                    currentProperty.MultipleOf = multipleOfAttribute.Value;
                                    break;
                            }

                            break;
                        case JSchemaType.String:
                        case JSchemaType.String | JSchemaType.Null:
                            switch (propAttribute)
                            {
                                case Format format:
                                    currentProperty.Format = format.Value;
                                    break;
                            }

                            break;
                        case JSchemaType.Array:
                        case JSchemaType.Array | JSchemaType.Null:
                            switch (propAttribute)
                            {
                                case DisallowAdditionalItemsAttribute:
                                    currentProperty.AllowAdditionalItems = false;
                                    break;
                                case JsonPropertyAttribute jsonPropertyAttribute:
                                    if (jsonPropertyAttribute.Required == Required.AllowNull)
                                    {
                                        _schema.Properties[propName] =
                                            AddNullTypeToPropertyIfRequired(currentProperty);
                                    }

                                    break;
                            }

                            break;
                        case JSchemaType.Object:
                        case JSchemaType.Object | JSchemaType.Null:
                            switch (propAttribute)
                            {
                                case JsonPropertyAttribute jsonPropertyAttribute:
                                    if (jsonPropertyAttribute.Required == Required.AllowNull)
                                    {
                                        _schema.Properties[propName] =
                                            AddNullTypeToPropertyIfRequired(currentProperty);
                                    }
                                    break;
                            }

                            break;
                    }
                }
            }
        }
    }
}