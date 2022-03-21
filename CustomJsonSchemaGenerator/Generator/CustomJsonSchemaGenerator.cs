using System;
using System.Collections.Generic;
using System.Linq;
using CustomJsonSchemaGenerator.Generator.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using static CustomJsonSchemaGenerator.Generator.Helpers.CustomJsonSchemaGeneratorHelper;

namespace CustomJsonSchemaGenerator.Generator
{
    public class CustomJSchemaGenerator
    {
        private static CustomJSchemaGenerator _instance;
        private readonly JSchema _globalJSchema;

        private CustomJSchemaGenerator()
        {
            var generator = new JSchemaGenerator
            {
                SchemaIdGenerationHandling = SchemaIdGenerationHandling.FullTypeName,
                SchemaReferenceHandling = SchemaReferenceHandling.None,
                DefaultRequired = Required.Default
            };

            var globalType = CustomTypeBuilder.BuildTypeWithTypesToGenerateJsonSchemaByAttribute();

            _globalJSchema = generator.Generate(globalType);

            UpdateGlobalJSchemaAccordingToCustomAttributes(globalType, ref _globalJSchema);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="typeOfObjectToGetSchemaFor">Type the schema was generated for</param>
        /// <returns></returns>
        public static JSchema GetJsonSchema(Type typeOfObjectToGetSchemaFor, params Attribute[] attributes)

        {
            _instance ??= new CustomJSchemaGenerator();

            Dictionary<Type, dynamic> attributesWithValues = null;
            if (attributes.Any())
            {
                attributesWithValues =
                    attributes.ToDictionary(
                        keySelector: attr => attr.GetType(),
                        elementSelector: attr => ((dynamic)attr).Value);
            }

            var typeId = RandomHelper.GenerateIdForType(typeOfObjectToGetSchemaFor, attributesWithValues);

            return _instance._globalJSchema.Properties[typeId];
        }
    }

    public class AttributeWithValue
    {
        public Type AttributeType { get; }
        public dynamic Value { get; }

        public AttributeWithValue(Type attributeType, dynamic value)
        {
            AttributeType = attributeType;
            Value = value;
        }
    }
}