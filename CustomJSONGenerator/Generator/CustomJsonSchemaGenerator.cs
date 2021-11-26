using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                SchemaIdGenerationHandling = SchemaIdGenerationHandling.TypeName
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
                .Where(type => Attribute.IsDefined(type, typeof(GenerateJSONSchemaAttribute)))
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
            var typesWithAttributesAndPropsWithAttributes =
                new Dictionary<Type, TypeAttributesAndPropsWithAttributes>();
            GetCustomAttributesFromTypeAndItsProps(context.ObjectType, typesWithAttributesAndPropsWithAttributes);

            foreach (var (type, typeAttributesAndPropsWithAttributes) in typesWithAttributesAndPropsWithAttributes)
            {
                GenerateSchema(context, type, typeAttributesAndPropsWithAttributes);
            }

            return context.Generator.Generate(context.ObjectType);
        }

        public override bool CanGenerateSchema(JSchemaTypeGenerationContext context) =>
            context.ObjectType.Namespace != "System";

        private static void GetCustomAttributesFromTypeAndItsProps(Type baseType,
            IDictionary<Type, TypeAttributesAndPropsWithAttributes> typesWithAttributesAndItsPropsWithAttributes)
        {
            // Getting base type attributes. Adding to Dictionary if attrs were found
            var baseTypeCustomAttributes = GetCustomAttributesListFromType(baseType);
            TypeAttributesAndPropsWithAttributes typeAttributesAndPropsWithAttributes;
            if (baseTypeCustomAttributes != null)
            {
                typeAttributesAndPropsWithAttributes =
                    new TypeAttributesAndPropsWithAttributes().AddTypeAttributes(baseTypeCustomAttributes);
                typesWithAttributesAndItsPropsWithAttributes.Add(baseType, typeAttributesAndPropsWithAttributes);
            }

            // Getting all properties of baseType
            var properties = baseType.GetProperties();

            // Loop through found properties
            foreach (var prop in properties)
            {
                // Getting name and attributes of the current property
                var curPropNameAndAttributes = GetPropNameAndCustomAttributes(prop);
                if (curPropNameAndAttributes != null)
                {
                    // Add property with attributes if they were found
                    try
                    {
                        typesWithAttributesAndItsPropsWithAttributes[baseType]
                            .AddPropsWithAttributes(curPropNameAndAttributes.Item1, curPropNameAndAttributes.Item2);
                    }
                    catch (KeyNotFoundException)
                    {
                        typeAttributesAndPropsWithAttributes = new TypeAttributesAndPropsWithAttributes()
                            .AddPropsWithAttributes(curPropNameAndAttributes.Item1, curPropNameAndAttributes.Item2);
                        typesWithAttributesAndItsPropsWithAttributes.Add(baseType,
                            typeAttributesAndPropsWithAttributes);
                    }
                }

                var curPropType = prop.PropertyType;
                if (curPropType.Namespace == "System" ||
                    typesWithAttributesAndItsPropsWithAttributes.ContainsKey(curPropType)) continue;

                // Invoke method to get attributes of a property type if they presented
                GetCustomAttributesFromTypeAndItsProps(curPropType, typesWithAttributesAndItsPropsWithAttributes);
            }
        }

        private static Tuple<string, List<JsonSchemaPropAttribute>> GetPropNameAndCustomAttributes(
            PropertyInfo prop)
        {
            var customAttributes = prop.GetCustomAttributes(typeof(JsonSchemaPropAttribute), false);
            if (customAttributes.Length == 0) return null;

            var customJsonSchemaPropAttributes = customAttributes
                .Select(el => (JsonSchemaPropAttribute) el).ToList();

            var propName = GetPropNameFromJsonPropertyAttribute(prop);

            return new Tuple<string, List<JsonSchemaPropAttribute>>(propName, customJsonSchemaPropAttributes);
        }

        private static string GetPropNameFromJsonPropertyAttribute(PropertyInfo property) =>
            ((JsonPropertyAttribute) property.GetCustomAttributes(typeof(JsonPropertyAttribute), false)[0])
            .PropertyName;

        private static List<JsonSchemaTypeAttribute> GetCustomAttributesListFromType(Type type)
        {
            var typeCustomAttributes = type.GetCustomAttributes(typeof(JsonSchemaTypeAttribute), false)
                .Select(el => (JsonSchemaTypeAttribute) el).ToList();

            return typeCustomAttributes.Count > 0 ? typeCustomAttributes : null;
        }

        private static bool IsIntegerNumber(Type type) => new[]
        {
            typeof(byte),
            typeof(short),
            typeof(int),
            typeof(long)
        }.Contains(type);

        private static bool IsFloatNumber(Type type) => new[]
        {
            typeof(float),
            typeof(double),
            typeof(decimal)
        }.Contains(type);


    private static JSchema _schema;
        private static void GenerateSchema(JSchemaTypeGenerationContext context,
            Type type, TypeAttributesAndPropsWithAttributes typeAttributesAndItsPropsWithAttributes)
        {
            _schema = context.Generator.Generate(type);

            HandleNumberJsonProperties(type);

            AddCustomPropertiesToJsonTypesIfExist(typeAttributesAndItsPropsWithAttributes);

            AddCustomPropertiesToJsonPropertiesIfExist(typeAttributesAndItsPropsWithAttributes);
        }

        private static void HandleNumberJsonProperties(Type type)
        {
            foreach (var prop in type.GetProperties())
            {
                var isInteger = IsIntegerNumber(prop.PropertyType);
                var isFloat = IsFloatNumber(prop.PropertyType);

                if (isInteger || isFloat)
                {
                    var propName = GetPropNameFromJsonPropertyAttribute(prop);
                    _schema.Properties[propName].Type = isInteger ? JSchemaType.Integer : JSchemaType.Number;
                }
            }
        }

        private static void AddCustomPropertiesToJsonTypesIfExist(TypeAttributesAndPropsWithAttributes typeAttributesAndItsPropsWithAttributes)
        {
            if (typeAttributesAndItsPropsWithAttributes.Attributes == null) return;

            // Iterate through type attributes
            foreach (var typeAttribute in typeAttributesAndItsPropsWithAttributes.Attributes)
            {
                switch (typeAttribute)
                {
                    case MaximumPropertiesAttribute maxProperties:
                        _schema.MaximumProperties = (long)maxProperties.Value;
                        break;
                    case MinimumPropertiesAttribute minProperties:
                        _schema.MinimumProperties = (long)minProperties.Value;
                        break;
                    case AllowAdditionalPropertiesAttribute allowAdditionalProperties:
                        _schema.AllowAdditionalProperties = allowAdditionalProperties.Value;
                        break;
                }
            }
        }

        private static void AddCustomPropertiesToJsonPropertiesIfExist(
            TypeAttributesAndPropsWithAttributes typeAttributesAndItsPropsWithAttributes)
        {
            if (typeAttributesAndItsPropsWithAttributes.PropertiesWithAttributes == null) return;

            foreach (var (propName, propAttributes) in
                typeAttributesAndItsPropsWithAttributes.PropertiesWithAttributes)
            {
                foreach (var propAttribute in propAttributes)
                {
                    var currentProperty = _schema.Properties[propName];
                    var currentPropertyType = currentProperty.Type;

                    switch (currentPropertyType)
                    {
                        case JSchemaType.Number:
                        case JSchemaType.Integer:
                            switch (propAttribute)
                            {
                                case MaximumAttribute maximumAttribute:
                                    currentProperty.Maximum = maximumAttribute.Value;
                                    break;
                                case MinimumAttribute minimumAttribute:
                                    currentProperty.Minimum = minimumAttribute.Value;
                                    break;
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
                            switch (propAttribute)
                            {
                                case MaximumLengthAttribute maxLengthAttribute:
                                    currentProperty.MaximumLength = (long)maxLengthAttribute.Value;
                                    break;
                                case MinimumLengthAttribute minLengthAttribute:
                                    currentProperty.MinimumLength = (long)minLengthAttribute.Value;
                                    break;
                                case StringFormat stringFormat:
                                    currentProperty.Format = stringFormat.Value;
                                    break;
                            }

                            break;
                        case JSchemaType.Array:
                            switch (propAttribute)
                            {
                                case MinimumItemsAttribute minItemsAttribute:
                                    currentProperty.MinimumItems = (long)minItemsAttribute.Value;
                                    break;
                                case MaximumItemsAttribute maxItemsAttribute:
                                    currentProperty.MaximumItems = (long)maxItemsAttribute.Value;
                                    break;
                                case AllowAdditionalItemsAttribute allowAdditionalItemsAttribute:
                                    currentProperty.AllowAdditionalItems = allowAdditionalItemsAttribute.Value;
                                    break;
                            }

                            break;
                    }
                }
            }
        }
    }

    // Class stores Attributes of a type and all its props names with their attributes if presented
    internal class TypeAttributesAndPropsWithAttributes
    {
        internal List<JsonSchemaTypeAttribute> Attributes;
        internal Dictionary<string, List<JsonSchemaPropAttribute>> PropertiesWithAttributes;

        internal TypeAttributesAndPropsWithAttributes AddTypeAttributes(
            IEnumerable<JsonSchemaTypeAttribute> typeAttributes)
        {
            Attributes = new List<JsonSchemaTypeAttribute>(typeAttributes);
            return this;
        }

        internal TypeAttributesAndPropsWithAttributes AddPropsWithAttributes(string propName,
            List<JsonSchemaPropAttribute> attributes)
        {
            PropertiesWithAttributes ??= new Dictionary<string, List<JsonSchemaPropAttribute>>();
            PropertiesWithAttributes.Add(propName, attributes);

            return this;
        }
    }
}