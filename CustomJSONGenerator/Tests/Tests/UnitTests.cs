using System;
using System.IO;
using CustomJSONGenerator.Tests.Models;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using static CustomJSONGenerator.Generator.CustomJsonSchemaGenerator;

namespace CustomJSONGenerator.Tests.Tests
{
    public class UnitTests
    {
        [Test]
        public void VerifySimpleStringWithMinimumLengthRestrictionJSchemaGeneration()
        {
            // var expectedJsonSchema = GetJsonObjectFromJsonFile("MeetingSyncResponseWithResult.json").ToString();
            var actualJsonSchema = GetJsonSchema(typeof(SimpleStringFields.SimpleStringWithMinimumLengthRestriction).FullName).ToString();
            Console.WriteLine();
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