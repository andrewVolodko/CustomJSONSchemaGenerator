using System;
using System.Collections.Generic;
using System.IO;
using CustomJsonSchemaGenerator.Generator;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;

namespace CustomJsonSchemaGenerator.Demo
{
    public class DemoTest
    {
        [Test]
        public void JsonSchemaGenerationAndValidationDemo()
        {
            // Getting "response"
            var jsonData = GetJsonObjectFromJsonFile("Demo/parent.json").ToString();

            // Getting JSON-Schema
            // var schema = CustomJSchemaGenerator.GetJsonSchema(typeof(Parent));
            //
            // // Validating JSON against JSON-Schema
            // var result = ValidateJsonSchema(jsonData, schema, out var jsonSchemaValidationErrors);
            //
            // Assert.IsTrue(result);
        }

        private static bool ValidateJsonSchema(string jsonData, JSchema schema, out IList<ValidationError> errors)
        {
            var jObjectData = JObject.Parse(jsonData);
            return jObjectData.IsValid(schema, out errors);
        }

        private static JObject GetJsonObjectFromJsonFile(string jsonFile)
        {
            string fileString;
            using (var sw = new StreamReader(jsonFile))
            {
                try
                {
                    fileString = sw.ReadToEnd();
                    sw.Close();
                }
                catch (FileNotFoundException)
                {
                    throw new FileNotFoundException("Configuration file was NOT found.");
                }
            }

            return JObject.Parse(fileString);
        }
    }
}