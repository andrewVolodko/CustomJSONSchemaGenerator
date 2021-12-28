using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace CustomJsonSchemaGenerator.Generator.Helpers
{
    internal static class CustomJsonSchemaGeneratorHelper
    {
        internal static void UpdateGlobalJSchemaAccordingToCustomAttributes(Type globalType, ref JSchema _globalJSchema)
        {
            foreach (var globalTypeProperty in globalType.GetProperties())
            {
                var globalTypePropertySchema = _globalJSchema.Properties[globalTypeProperty.Name];
                LoopThroughTypeMembersAndUpdateSchema(globalTypeProperty.PropertyType, ref globalTypePropertySchema);
            }
        }

        /// <summary>
        /// Recursive method to loop through all properties of provided type
        /// Obtain custom type and field / property attributes
        /// Update current json property schema according to the obtained attribute value
        /// </summary>
        /// <param name="type">The type custom attributes need to be obtained from</param>
        /// <param name="schema">Already generated schema that should be modified</param>
        /// <param name="arrayItemsCannotBeNullAttribute">Optional parameter that is used ONLY in cases
        /// when enumerable type is processing
        /// The problem is that it's necessary to set array item nullable parameter
        /// but array - is a property, and its item - object
        /// and it's necessary to specify PROPERTY attribute to OBJECT
        /// so that this argument was added to the method to handle the case</param>
        private static void LoopThroughTypeMembersAndUpdateSchema(Type type, ref JSchema schema,
            Attribute arrayItemsCannotBeNullAttribute = null)
        {
            // Declare list with type custom attributes at the beginning
            // since it will be possibly filled before obtaining the type custom attributes
            var typeCustomAttributes = new List<Attribute>();

            // It's necessary to handle arrays, since they have type "inside" - type of items
            JSchema arrayPropertySchema = null;
            if (type.GetInterface(nameof(IEnumerable)) != null)
            {
                arrayPropertySchema = schema;
                // Get array / list items type
                type = type.GetElementType() ?? type.GetGenericArguments().Single();
                schema = schema.Items[0];

                if (arrayItemsCannotBeNullAttribute != null)
                {
                    typeCustomAttributes.Add(arrayItemsCannotBeNullAttribute);
                }
            }

            // Get custom attributes of type
            typeCustomAttributes.AddRange(GetTypeCustomAttributes(type));

            SetJsonPropertyTypeConstraints(ref schema, typeCustomAttributes, arrayPropertySchema);

            // Obtain properties and fields of type
            var typeMembers = GetTypePropertiesAndFields(type);

            foreach (var typeMember in typeMembers)
            {
                var typeMemberPropertySchema = schema.Properties[GetMemberName(typeMember)];
                var typeMemberCustomAttributes = GetMemberCustomAttributes(typeMember);

                SetJsonPropertyPropertyConstraints(ref typeMemberPropertySchema, typeMemberCustomAttributes);

                // It's necessary to decide whether further type inner properties processing required
                // So that obtain property / field type
                var typeMemberType = typeMember is PropertyInfo propertyInfo
                    ? propertyInfo.PropertyType
                    : ((FieldInfo) typeMember).FieldType;

                // In case array contains system type items (e.g. string)
                // If so, it's not necessary to loop through system item properties
                var possibleSystemArrayItemsType =
                    typeMemberType.GetElementType() ?? typeMemberType.GetGenericArguments().SingleOrDefault();

                // If type is in System namespace, further processing not necessary, since it's the "simplest" type for json schema
                if (typeMemberType.Namespace == "System" ||
                    possibleSystemArrayItemsType?.Namespace == "System") continue;

                // Obtain ArrayItemsCannotBeNull attribute if exists
                arrayItemsCannotBeNullAttribute =
                    typeMemberCustomAttributes.SingleOrDefault(attr => attr is ArrayItemsCannotBeNullAttribute);

                LoopThroughTypeMembersAndUpdateSchema(typeMemberType, ref typeMemberPropertySchema,
                    arrayItemsCannotBeNullAttribute);
            }
        }

        private static string GetMemberName(MemberInfo member)
        {
            var jsonPropertyAttributes = member.GetCustomAttributes(typeof(JsonPropertyAttribute), false);

            return jsonPropertyAttributes.Any()
                ? ((JsonPropertyAttribute) jsonPropertyAttributes.Single()).PropertyName ?? member.Name
                : member.Name;
        }

        private static List<Attribute> GetMemberCustomAttributes(MemberInfo member) =>
            member.GetCustomAttributes(false).Cast<Attribute>().ToList();

        private static List<Attribute> GetTypeCustomAttributes(Type type) =>
            type.GetCustomAttributes(false).Cast<Attribute>().ToList();

        private static List<MemberInfo> GetTypePropertiesAndFields(Type type) =>
            type.Namespace == "System" ?
                new List<MemberInfo>() :
                type.GetMembers().Where(member => member is FieldInfo or PropertyInfo).ToList();

        private static void SetJsonPropertyTypeConstraints(ref JSchema typeSchema, List<Attribute> customAttributes,
            JSchema optionalArrayPropertySchema)
        {
            var currentSchema = typeSchema;
            if (optionalArrayPropertySchema != null)
            {
                currentSchema = optionalArrayPropertySchema.Items[0];
            }

            foreach (var customAttribute in customAttributes)
            {
                switch (customAttribute)
                {
                    case MaximumPropertiesAttribute maxProperties:
                        currentSchema.MaximumProperties = maxProperties.Value;
                        break;
                    case MinimumPropertiesAttribute minProperties:
                        currentSchema.MinimumProperties = minProperties.Value;
                        break;
                    case AllowAdditionalPropertiesAttribute allowAdditionalPropertiesAttribute:
                        currentSchema.AllowAdditionalProperties = allowAdditionalPropertiesAttribute.Value;
                        break;
                    case AdditionalPropertiesAttribute additionalPropertiesAttribute:
                        currentSchema.AdditionalProperties = additionalPropertiesAttribute.Value;
                        break;
                    case ArrayItemsCannotBeNullAttribute arrayItemsCannotBeNullAttribute:
                        if (arrayItemsCannotBeNullAttribute.Value && optionalArrayPropertySchema != null)
                        {
                            currentSchema.Type = JSchemaType.Object;
                        }

                        break;
                }
            }
        }

        private static void SetJsonPropertyPropertyConstraints(ref JSchema propertySchema,
            List<Attribute> customAttributes)
        {
            foreach (var customAttribute in customAttributes)
            {
                switch (propertySchema.Type)
                {
                    case JSchemaType.Number:
                    case JSchemaType.Integer:
                    case JSchemaType.Number | JSchemaType.Null:
                    case JSchemaType.Integer | JSchemaType.Null:
                        switch (customAttribute)
                        {
                            case ExclusiveMaximumAttribute exclusiveMaximumAttribute:
                                propertySchema.ExclusiveMaximum = exclusiveMaximumAttribute.Value;
                                break;
                            case ExclusiveMinimumAttribute exclusiveMinimumAttribute:
                                propertySchema.ExclusiveMinimum = exclusiveMinimumAttribute.Value;
                                break;
                            case MultipleOfAttribute multipleOfAttribute:
                                propertySchema.MultipleOf = multipleOfAttribute.Value;
                                break;
                        }

                        break;
                    case JSchemaType.String:
                    case JSchemaType.String | JSchemaType.Null:
                        switch (customAttribute)
                        {
                            case FormatAttribute format:
                                propertySchema.Format = format.Value;
                                break;
                        }

                        break;
                    case JSchemaType.Array:
                    case JSchemaType.Array | JSchemaType.Null:
                        switch (customAttribute)
                        {
                            case AllowAdditionalItemsAttribute additionalItemsAttribute:
                                propertySchema.AllowAdditionalItems = additionalItemsAttribute.Value;
                                break;
                            case ContainsAttribute containsAttribute:
                                propertySchema.Contains = containsAttribute.Value;
                                break;
                            case MaxContainsAttribute maxContainsAttribute:
                                if (customAttributes.Any(attr => attr is ContainsAttribute))
                                {
                                    propertySchema.MaximumContains = maxContainsAttribute.Value;
                                }

                                break;
                            case MinContainsAttribute minContainsAttribute:
                                if (customAttributes.Any(attr => attr is ContainsAttribute))
                                {
                                    propertySchema.MinimumContains = minContainsAttribute.Value;
                                }

                                break;
                            case UniqueItemsAttribute uniqueItemsAttribute:
                                propertySchema.UniqueItems = uniqueItemsAttribute.Value;
                                break;
                        }

                        break;
                }
            }
        }
    }
}