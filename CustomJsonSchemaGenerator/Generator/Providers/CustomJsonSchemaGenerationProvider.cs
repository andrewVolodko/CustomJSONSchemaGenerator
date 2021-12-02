using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace CustomJsonSchemaGenerator.Generator.Providers
{
    // Class to extend JSchemaGenerationProvider functionality. E.g. add more jsonSchema field and object attributes
    internal class CustomJsonSchemaGenerationProvider : JSchemaGenerationProvider
    {
        public override JSchema GetSchema(JSchemaTypeGenerationContext context) =>
            context.Generator.Generate(context.ObjectType);

        public override bool CanGenerateSchema(JSchemaTypeGenerationContext context) =>
            context.ObjectType.Namespace != "System";
    }
}