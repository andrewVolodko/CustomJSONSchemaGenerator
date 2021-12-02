using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using CustomJsonSchemaGenerator.Generator.Helpers;
using CustomJsonSchemaGenerator.Generator.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace CustomJsonSchemaGenerator.Generator
{
    public class CustomJSchemaGenerator
    {
        private static CustomJSchemaGenerator _instance;
        private readonly JSchema _globalJsonSchema;

        private CustomJSchemaGenerator()
        {
            var generator = new JSchemaGenerator
            {
                // GenerationProviders = {new CustomJsonSchemaGenerationProvider()},
                SchemaIdGenerationHandling = SchemaIdGenerationHandling.FullTypeName,
                SchemaReferenceHandling = SchemaReferenceHandling.None,
                DefaultRequired = Required.Default
            };

            var globalType = CustomTypeBuilder.BuildTypeWithTypesToGenerateJsonSchemaByAttribute(
                typeof(GenerateJsonSchemaAttribute),
                "TypesToCreateJSchema");

            _globalJsonSchema = generator.Generate(globalType);

            foreach (var globalTypeProperty in globalType.GetProperties())
            {
                var globalTypePropertySchema = _globalJsonSchema.Properties[globalTypeProperty.Name];
                LoopThroughTypeMembersAndUpdateSchema(globalTypeProperty.PropertyType, ref globalTypePropertySchema);
            }
        }

        private static void LoopThroughTypeMembersAndUpdateSchema(Type type, ref JSchema schema)
        {
            JSchema arrayPropertySchema = null;
            if (type.GetInterface(nameof(IEnumerable)) != null)
            {
                arrayPropertySchema = schema;
                type = type.GetElementType() ?? type.GetGenericArguments().Single();
                schema = schema.Items[0];
            }

            var typeCustomAttributes = GetTypeCustomAttributes(type);

            SetJsonPropertyTypeConstraints(ref schema, typeCustomAttributes, arrayPropertySchema);

            var typeMembers = GetTypePropertiesAndFields(type);

            foreach (var typeMember in typeMembers)
            {
                var typeMemberPropertySchema = schema.Properties[GetMemberName(typeMember)];
                var typeMemberCustomAttributes = GetMemberCustomAttributes(typeMember);

                SetJsonPropertyPropertyConstraints(ref typeMemberPropertySchema, typeMemberCustomAttributes);

                var typeMemberType = typeMember is PropertyInfo propertyInfo
                    ? propertyInfo.PropertyType
                    : ((FieldInfo) typeMember).FieldType;

                if (typeMemberType.Namespace == "System") continue;

                LoopThroughTypeMembersAndUpdateSchema(typeMemberType, ref typeMemberPropertySchema);
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
                    case DisallowAdditionalPropertiesAttribute:
                        currentSchema.AllowAdditionalProperties = false;
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
                            case ExclusiveMaximumAttribute:
                                propertySchema.ExclusiveMaximum = true;
                                break;
                            case ExclusiveMinimumAttribute:
                                propertySchema.ExclusiveMinimum = true;
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
                            case Format format:
                                propertySchema.Format = format.Value;
                                break;
                        }

                        break;
                    case JSchemaType.Array:
                    case JSchemaType.Array | JSchemaType.Null:
                        switch (customAttribute)
                        {
                            case DisallowAdditionalItemsAttribute:
                                propertySchema.AllowAdditionalItems = false;
                                break;
                            // case JsonPropertyAttribute jsonPropertyAttribute:
                            //     if (jsonPropertyAttribute.NullValueHandling == NullValueHandling.Ignore)
                            //     {
                            //         propertySchema.Type = JSchemaType.Array | JSchemaType.Null;
                            //     }
                            //
                            //     break;
                        }

                        break;
                    // case JSchemaType.Object:
                    // case JSchemaType.Object | JSchemaType.Null:
                    //     switch (customAttribute)
                    //     {
                    //         case JsonPropertyAttribute jsonPropertyAttribute:
                    //             if (jsonPropertyAttribute.NullValueHandling == NullValueHandling.Ignore)
                    //             {
                    //                 propertySchema.Type = JSchemaType.Object | JSchemaType.Null;
                    //             }
                    //
                    //             break;
                    //     }
                    //
                    //     break;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="jsonSchemaName">FullName of the type the schema was generated for</param>
        /// <returns></returns>
        public static JSchema GetJsonSchema(string jsonSchemaName)
        {
            _instance ??= new CustomJSchemaGenerator();
            return _instance._globalJsonSchema.Properties[jsonSchemaName];
        }
    }
}