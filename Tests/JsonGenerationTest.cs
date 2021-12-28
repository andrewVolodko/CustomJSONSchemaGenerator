using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using CustomJsonSchemaGenerator.Generator;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using Tests.Model.Meetings;

namespace Tests
{
    [TestFixture]
    public class JsonGenerationTest
    {
        [Test]
        public void Test1()
        {
            var jsonData = GetJsonObjectFromJsonFile("MeetingSyncResponseWithResult.json").ToString();
            var schema = CustomJSchemaGenerator
                .GetJsonSchema(typeof(Meeting),
                    new MaxLengthAttribute(6567),
                    new FormatAttribute("weweferfw"));

            // var test = schema.ToString();
            // var result = ValidateJsonSchema(jsonData, schema, out var jsonSchemaValidationErrors);

            var schema1 = CustomJSchemaGenerator
                .GetJsonSchema(typeof(Meeting));

            // Console.WriteLine(result);
        }

        public static bool ValidateJsonSchema(string jsonData, JSchema schema, out IList<ValidationError> errors) // Add errors
        {
            var jObjectData = JObject.Parse(jsonData);
            return jObjectData.IsValid(schema, out errors);
        }

        public static JObject GetJsonObjectFromJsonFile(string jsonFile)
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