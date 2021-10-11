using System;
using System.Collections.Generic;
using System.IO;
using CustomJSONGenerator;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using TestPart.Model.Meetings;

namespace TestPart
{
    [TestFixture]
    public class JsonGenerationTest
    {
        [Test]
        public void Test1()
        {
            var jsonData = GetJsonObjectFromJsonFile("MeetingSyncResponse.json").ToString();
            JSchema schema = JSONSchemaGenerator.GetJSONSchema(typeof(MeetingSync).FullName);

            var result = ValidateJsonSchema(jsonData, schema, out var jsonSchemaValidationErrors);

            Console.WriteLine();
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