using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using RoomBot.Infrastructure.Helpers;

namespace CustomJSONGenerator
{
    public class JSONSchemaGenerator
    {
        private static JSONSchemaGenerator _instance;
        private readonly JSchema _globalJSONSchema;

        private JSONSchemaGenerator()
        {
            var generator = new JSchemaGenerator
            {
                GenerationProviders = {new ObjectsSchemaGenerationProvider()},
                SchemaIdGenerationHandling = SchemaIdGenerationHandling.TypeName
            };

            var type = BuildTypeWithTypesToGenerateJSONSchema();
            _globalJSONSchema = generator.Generate(type);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="jsonSchemaName">FullName of the type the schema was generated for</param>
        /// <returns></returns>
        public static JSchema GetJSONSchema(string jsonSchemaName)
        {
            _instance ??= new JSONSchemaGenerator();
            return _instance._globalJSONSchema.Properties[jsonSchemaName];
        }

        private static Type BuildTypeWithTypesToGenerateJSONSchema()
        {
            var types = new Dictionary<string, Type>();

            var typesToGenerateJSONSchema = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assemble => assemble.GetTypes())
                .Where(type =>
                    Attribute.IsDefined(type, typeof(GenerateJSONSchemaAttribute)))
                .ToList();

            foreach (var returnType in typesToGenerateJSONSchema)
            {
                var fullName = returnType.FullName;
                if (!types.ContainsKey(fullName!))
                    types.Add(fullName, returnType);
            }

            var classBuilder = new DynamicClassBuilder("TypesToCreateJSchema");
            return classBuilder.CreateType(types.Keys.ToArray(), types.Values.ToArray());
        }

        // public static bool ValidateJsonSchema<T>(string jsonData, out IList<ValidationError> errors) // Add errors
        // {
        //     // Create class that contains all props of all types to create json-schema.
        //     // It's necessary to do this way because of the JSchemaGenerator restriction of 10 schema generations per hour
        //     // Doing this way JSchemaGenerator generates only one big schema with sub-schemas inside
        //     _generatedJSchema ??= GenerateJSchema(BuildTypeWithTypesToGenerateJSchema());
        //
        //     var schema = _generatedJSchema.Properties[typeof(T).FullName!];
        //     var jObjectData = JObject.Parse(jsonData);
        //     return jObjectData.IsValid(schema, out errors);
        // }
        //
        // public static JObject GetJsonObjectFromJsonFile(string jsonFile)
        // {
        //     string fileString;
        //     using (var sw = new StreamReader(jsonFile))
        //     {
        //         try
        //         {
        //             fileString = sw.ReadToEnd();
        //             sw.Close();
        //         }
        //         catch (FileNotFoundException)
        //         {
        //             throw new FileNotFoundException("Configuration file was NOT found.");
        //         }
        //     }
        //
        //     return JObject.Parse(fileString);
        // }
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

        public override bool CanGenerateSchema(JSchemaTypeGenerationContext context)
        {
            return context.ObjectType.Namespace != "System";
        }

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

        private static Tuple<string, List<JSONSchemaPropAttribute>> GetPropNameAndCustomAttributes(
            PropertyInfo prop)
        {
            var customAttributes = prop.GetCustomAttributes(typeof(JSONSchemaPropAttribute), false);
            if (customAttributes.Length == 0) return null;

            var customJsonSchemaPropAttributes = customAttributes
                .Select(el => (JSONSchemaPropAttribute) el).ToList();

            var propName = GetPropNameFromJsonPropertyAttribute(prop);

            return new Tuple<string, List<JSONSchemaPropAttribute>>(propName, customJsonSchemaPropAttributes);
        }

        private static string GetPropNameFromJsonPropertyAttribute(PropertyInfo prop) =>
            ((JsonPropertyAttribute) prop.GetCustomAttributes(typeof(JsonPropertyAttribute), false)[0])
            .PropertyName;

        private static List<JSONSchemaTypeAttribute> GetCustomAttributesListFromType(Type type)
        {
            var typeCustomAttributes = type.GetCustomAttributes(typeof(JSONSchemaTypeAttribute), false)
                .Select(el => (JSONSchemaTypeAttribute) el).ToList();

            return typeCustomAttributes.Count > 0 ? typeCustomAttributes : null;
        }

        private static bool IsIntegerNumber(Type type)
        {
            return new[]
            {
                typeof(sbyte),
                typeof(byte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong)
            }.Contains(type);
        }

        private static void GenerateSchema(JSchemaTypeGenerationContext context,
            Type type, TypeAttributesAndPropsWithAttributes typeAttributesAndItsPropsWithAttributes)
        {
            var schema = context.Generator.Generate(type);

            foreach (var prop in type.GetProperties())
            {
                if (!IsIntegerNumber(prop.PropertyType)) continue;
                var propName = GetPropNameFromJsonPropertyAttribute(prop);
                var schemaProp = schema.Properties[propName];
                schemaProp.Type = JSchemaType.Number;
                schemaProp.Format = prop.PropertyType.Name.ToLower();
            }

            if (typeAttributesAndItsPropsWithAttributes.Attributes != null)
            {
                // Iterate through type attributes
                foreach (var typeAttribute in typeAttributesAndItsPropsWithAttributes.Attributes)
                {
                    switch (typeAttribute)
                    {
                        case MaximumPropertiesAttribute maxProperties:
                            schema.MaximumProperties = maxProperties.Value;
                            break;
                        case MinimumPropertiesAttribute minProperties:
                            schema.MinimumProperties = minProperties.Value;
                            break;
                        case AllowAdditionalPropertiesAttribute allowAdditionalProperties:
                            schema.AllowAdditionalProperties = allowAdditionalProperties.Value;
                            break;
                    }
                }
            }

            if (typeAttributesAndItsPropsWithAttributes.PropertiesWithAttributes == null) return;
            foreach (var (propName, propAttributes) in typeAttributesAndItsPropsWithAttributes.PropertiesWithAttributes)
            {
                foreach (var propAttribute in propAttributes)
                {
                    var curProp = schema.Properties[propName];
                    var curPropType = curProp.Type;

                    switch (curPropType)
                    {
                        case JSchemaType.Number:
                        case JSchemaType.Integer:
                            switch (propAttribute)
                            {
                                case MaximumAttribute maximumAttribute:
                                    curProp.Maximum = maximumAttribute.Value;
                                    break;
                                case MinimumAttribute minimumAttribute:
                                    curProp.Minimum = minimumAttribute.Value;
                                    break;
                            }
                            break;
                        case JSchemaType.String:
                            switch (propAttribute)
                            {
                                case MaximumStringLengthAttribute maxLengthAttribute:
                                    curProp.MaximumLength = maxLengthAttribute.Value;
                                    break;
                                case MinimumStringLengthAttribute minLengthAttribute:
                                    curProp.MinimumLength = minLengthAttribute.Value;
                                    break;
                                case StringFormat stringFormat:
                                    curProp.Format = stringFormat.Value;
                                    break;
                            }
                            break;
                        case JSchemaType.Array:
                            switch (propAttribute)
                            {
                                case MinimumItemsAttribute minItemsAttribute:
                                    if (curPropType == JSchemaType.Array)
                                        curProp.MinimumItems = minItemsAttribute.Value;
                                    break;
                                case MaximumItemsAttribute maxItemsAttribute:
                                    if (curPropType == JSchemaType.Array)
                                        curProp.MaximumItems = maxItemsAttribute.Value;
                                    break;
                                case AllowAdditionalItemsAttribute allowAdditionalItemsAttribute:
                                    if (curPropType == JSchemaType.Array)
                                        curProp.AllowAdditionalItems = allowAdditionalItemsAttribute.Value;
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
        internal List<JSONSchemaTypeAttribute> Attributes;
        internal Dictionary<string, List<JSONSchemaPropAttribute>> PropertiesWithAttributes;

        internal TypeAttributesAndPropsWithAttributes AddTypeAttributes(
            IEnumerable<JSONSchemaTypeAttribute> typeAttributes)
        {
            Attributes = new List<JSONSchemaTypeAttribute>(typeAttributes);
            return this;
        }

        internal TypeAttributesAndPropsWithAttributes AddPropsWithAttributes(string propName,
            List<JSONSchemaPropAttribute> attributes)
        {
            PropertiesWithAttributes ??= new Dictionary<string, List<JSONSchemaPropAttribute>>();
            PropertiesWithAttributes.Add(propName, attributes);

            return this;
        }
    }
}