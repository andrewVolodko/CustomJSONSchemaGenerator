using System;
using System.Collections.Generic;
using System.IO;
using CustomJSONGenerator.Generator;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using Tests.Model;
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
            var schema = CustomJsonSchemaGenerator.GetJsonSchema(typeof(Response<MeetingSync>).FullName);

            var test = schema.ToString();
            var result = ValidateJsonSchema(jsonData, schema, out var jsonSchemaValidationErrors);

            Console.WriteLine(result);
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