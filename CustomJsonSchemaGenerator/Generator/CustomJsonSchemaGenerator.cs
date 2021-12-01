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
                GenerationProviders = {new CustomJsonSchemaGenerationProvider()},
                SchemaIdGenerationHandling = SchemaIdGenerationHandling.FullTypeName,
                DefaultRequired = Required.Default
            };

            var type = CustomTypeBuilder.BuildTypeWithTypesToGenerateJsonSchemaByAttribute(
                typeof(GenerateJsonSchemaAttribute),
                "TypesToCreateJSchema");

            _globalJsonSchema = generator.Generate(type);
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