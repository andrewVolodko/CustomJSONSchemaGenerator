using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace CustomJSONGenerator.Generator
{
    public class CustomJsonSchemaGenerator
    {
        private static CustomJsonSchemaGenerator _instance;
        private readonly JSchema _globalJsonSchema;

        private CustomJsonSchemaGenerator()
        {
            var generator = new JSchemaGenerator
            {
                GenerationProviders = {new ObjectsSchemaGenerationProvider()},
                SchemaIdGenerationHandling = SchemaIdGenerationHandling.FullTypeName,
                DefaultRequired = Required.Default
            };

            var type = BuildTypeWithTypesToGenerateJsonSchema();
            _globalJsonSchema = generator.Generate(type);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="jsonSchemaName">FullName of the type the schema was generated for</param>
        /// <returns></returns>
        public static JSchema GetJsonSchema(string jsonSchemaName)
        {
            _instance ??= new CustomJsonSchemaGenerator();
            return _instance._globalJsonSchema.Properties[jsonSchemaName];
        }


        /// <summary>
        /// Obtaining all types which require JSON schemas, and generating one big common type
        /// with properties of types obtained.
        /// If obtained type is generic type, Generic Parameter Constraint HAVE to be specified.
        /// Algorithm searches for all types that inherit or implement the constraint and makes generic type
        /// E.g. there is Response<T> where T: IResponse
        /// MeetingObj implements IResponse
        /// So that algorithm will generate Response<MeetingObj> generic type, in order to generate such JSON schema in the future
        /// </summary>
        /// <returns>Common type with props of types for which JSON schemas required</returns>
        private static Type BuildTypeWithTypesToGenerateJsonSchema()
        {
            var types = new Dictionary<string, Type>();

            var assembleTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assemble => assemble.GetTypes()).ToList();

            var typesToGenerateJsonSchema = assembleTypes
                .Where(type => Attribute.IsDefined(type, typeof(GenerateJsonSchemaAttribute)))
                .ToList();

            foreach (var type in typesToGenerateJsonSchema)
            {
                string fullName;
                // If current type is generic, it's necessary to obtain all it's constraints
                // to understand which type could be used with this generic, and so that generate schema for it
                if (type.IsGenericType)
                {
                    var constraintType = type.GetGenericArguments()[0].GetGenericParameterConstraints()[0];
                    var typesImplOrInheritConstraint = assembleTypes
                        .Where(t => constraintType.IsAssignableFrom(t) && t.Name != constraintType.Name)
                        .ToList();

                    foreach (var constructedGenericType in typesImplOrInheritConstraint.Select(
                        typeImplOrInheritConstraint => type.MakeGenericType(typeImplOrInheritConstraint)))
                    {
                        fullName = constructedGenericType.FullName;
                        if (!types.ContainsKey(fullName!))
                            types.Add(fullName, constructedGenericType);
                    }
                }
                else
                {
                    fullName = type.FullName;
                    if (!types.ContainsKey(fullName!))
                        types.Add(fullName, type);
                }
            }

            var classBuilder = new DynamicClassBuilder("TypesToCreateJSchema");
            return classBuilder.CreateType(types.Keys.ToArray(), types.Values.ToArray());
        }
    }

    // Class to extend JSchemaGenerationProvider functionality. E.g. add more jsonSchema field and object attributes
    internal class ObjectsSchemaGenerationProvider : JSchemaGenerationProvider
    {
        public override JSchema GetSchema(JSchemaTypeGenerationContext context)
        {
            var typesWithAttributesAndMembersWithAttributes =
                new Dictionary<Type, TypeAttributesAndMembersWithAttributes>();

            GetCustomAttributesFromTypeAndItsMembers(context.ObjectType, ref typesWithAttributesAndMembersWithAttributes);

            foreach (var (type, typeAttributesAndMembersWithAttributes) in typesWithAttributesAndMembersWithAttributes)
            {
                GenerateSchema(context, type, typeAttributesAndMembersWithAttributes);
            }

            return context.Generator.Generate(context.ObjectType);
        }

        public override bool CanGenerateSchema(JSchemaTypeGenerationContext context) =>
            context.ObjectType.Namespace != "System";

        private static void GetCustomAttributesFromTypeAndItsMembers(Type baseType,
            ref Dictionary<Type, TypeAttributesAndMembersWithAttributes> typesWithAttributesAndItsMembersWithAttributes)
        {
            // Getting base type attributes. Adding to Dictionary if attrs were found
            var baseTypeCustomAttributes = GetCustomAttributesListFromType(baseType);

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

            void LoopThroughFoundMembers(IEnumerable<dynamic> members, ref Dictionary<Type, TypeAttributesAndMembersWithAttributes> typesWithItsMembersWithAttributes)
            {
                foreach (var member in members)
                {
                    // Getting name and attributes of the current property
                    Tuple<string, List<JsonSchemaPropAttribute>> curPropNameAndAttributes = GetMemberNameAndCustomAttributes(member);
                    if (curPropNameAndAttributes != null)
                    {
                        // Add property with attributes if they were found
                        try
                        {
                            typesWithItsMembersWithAttributes[baseType]
                                .AddMemberWithAttributes(curPropNameAndAttributes.Item1, curPropNameAndAttributes.Item2);
                        }
                        catch (KeyNotFoundException)
                        {
                            typeAttributesAndPropsWithAttributes = new TypeAttributesAndMembersWithAttributes()
                                .AddMemberWithAttributes(curPropNameAndAttributes.Item1, curPropNameAndAttributes.Item2);
                            typesWithItsMembersWithAttributes.Add(baseType,
                                typeAttributesAndPropsWithAttributes);
                        }
                    }

                    var curPropType = member is PropertyInfo ? member.PropertyType : member.FieldType;

                    if (curPropType.GetInterface(nameof(IEnumerable)) != null && curPropType != typeof(string))
                    {
                        var arrayPropWithRequiredAttribute = GetArrayMemberNameAndRequiredAttribute(member);
                        if (arrayPropWithRequiredAttribute != null)
                        {
                            try
                            {
                                typesWithItsMembersWithAttributes[baseType]
                                    .AddArrayMemberWithRequiredAttributes(arrayPropWithRequiredAttribute.Item1, arrayPropWithRequiredAttribute.Item2);
                            }
                            catch (KeyNotFoundException)
                            {
                                typeAttributesAndPropsWithAttributes = new TypeAttributesAndMembersWithAttributes()
                                    .AddArrayMemberWithRequiredAttributes(arrayPropWithRequiredAttribute.Item1, arrayPropWithRequiredAttribute.Item2);
                                typesWithItsMembersWithAttributes.Add(baseType,
                                    typeAttributesAndPropsWithAttributes);
                            }
                        }
                    }

                    if (curPropType.Namespace == "System" ||
                        typesWithItsMembersWithAttributes.ContainsKey(curPropType)) continue;

                    // Invoke method to get attributes of a property type if they presented
                    GetCustomAttributesFromTypeAndItsMembers(curPropType, ref typesWithItsMembersWithAttributes);
                }
            }
        }

        private static Tuple<string, Required> GetArrayMemberNameAndRequiredAttribute(MemberInfo member)
        {
            var customAttributes = member.GetCustomAttributes(typeof(JsonPropertyAttribute), false);
            if (customAttributes.Length == 0) return null;

            var requiredAttribute = customAttributes.Single<dynamic>().Required;

            var propName = GetMemberName(member);

            return new Tuple<string, Required>(propName, requiredAttribute);
        }

        private static Tuple<string, List<JsonSchemaPropAttribute>> GetMemberNameAndCustomAttributes(
            MemberInfo member)
        {
            var customAttributes = member.GetCustomAttributes(typeof(JsonSchemaPropAttribute), false);
            if (customAttributes.Length == 0) return null;

            var customJsonSchemaPropAttributes = customAttributes
                .Select(el => (JsonSchemaPropAttribute) el).ToList();

            var propName = GetMemberName(member);

            return new Tuple<string, List<JsonSchemaPropAttribute>>(propName, customJsonSchemaPropAttributes);
        }

        private static string GetMemberName(MemberInfo member)
        {
            var jsonPropertyAttributes = member.GetCustomAttributes(typeof(JsonPropertyAttribute), false);

            return jsonPropertyAttributes.Any()
                ? ((JsonPropertyAttribute) jsonPropertyAttributes.Single()).PropertyName ?? member.Name
                : member.Name;
        }

        private static List<JsonSchemaTypeAttribute> GetCustomAttributesListFromType(Type type)
        {
            var typeCustomAttributes = type.GetCustomAttributes(typeof(JsonSchemaTypeAttribute), false)
                .Select(el => (JsonSchemaTypeAttribute) el).ToList();

            return typeCustomAttributes.Count > 0 ? typeCustomAttributes : null;
        }

        private static JSchema _schema;
        private static void GenerateSchema(JSchemaTypeGenerationContext context,
            Type type, TypeAttributesAndMembersWithAttributes typeAttributesAndItsPropsWithAttributes)
        {
            _schema = context.Generator.Generate(type);

            HandleArrayPropertiesIfExist(typeAttributesAndItsPropsWithAttributes);

            AddCustomPropertiesToJsonTypesIfExist(typeAttributesAndItsPropsWithAttributes);

            AddCustomPropertiesToJsonPropertiesIfExist(typeAttributesAndItsPropsWithAttributes);
        }

        // This is just a "plug" in order to add ability of having nullable arrays in schema
        // This feature is presented in Newtonsoft lib, but does not work for some reason
        private static void HandleArrayPropertiesIfExist(TypeAttributesAndMembersWithAttributes typeAttributesAndItsPropsWithAttributes)
        {
            if (typeAttributesAndItsPropsWithAttributes.ArrayMembersNamesWithRequiredAttribute == null) return;

            foreach (var (propName, requiredAttr) in
                typeAttributesAndItsPropsWithAttributes.ArrayMembersNamesWithRequiredAttribute)
            {
                if (requiredAttr != Required.AllowNull) continue;

                const string pattern = "(?<=\"\\$id\"[\\S\\s]+\"type\": ).+(?=,\\n.*items)";

                var stringSchema = _schema.Properties[propName].ToString();

                var fixedStringSchema = Regex.Replace(stringSchema, pattern, "[\"array\", \"null\"]");

                _schema.Properties[propName] = JSchema.Parse(fixedStringSchema);
            }
        }

        private static void AddCustomPropertiesToJsonTypesIfExist(TypeAttributesAndMembersWithAttributes typeAttributesAndItsPropsWithAttributes)
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
                        case JSchemaType.Number :
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
                        case JSchemaType.Array:
                        case JSchemaType.Array | JSchemaType.Null:
                            switch (propAttribute)
                            {
                                case DisallowAdditionalItemsAttribute:
                                    currentProperty.AllowAdditionalItems = false;
                                    break;
                            }

                            break;
                    }
                }
            }
        }
    }

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