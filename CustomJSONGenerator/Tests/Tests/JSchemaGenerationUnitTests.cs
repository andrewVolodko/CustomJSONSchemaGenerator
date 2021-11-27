using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CustomJSONGenerator.Tests.Models;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using static CustomJSONGenerator.Generator.CustomJsonSchemaGenerator;

namespace CustomJSONGenerator.Tests.Tests
{
    public class JSchemaGenerationUnitTests
    {
        private const string ExpectedJsonSchemasFolderName = "Tests/ExpectedJsonSchemas";

        [Test]
        [TestCaseSource(nameof(GetClassesNamesToGenerateJSchema))]
        public void VerifySimpleStringWithJsonPropertyName(Type classToGenerateJSchema)
        {
            var expectedJsonSchema = GetJsonObjectFromJsonFile($"{ExpectedJsonSchemasFolderName}/{classToGenerateJSchema.Name}.json").ToString();
            var actualJsonSchema = GetJsonSchema(classToGenerateJSchema.FullName).ToString();

            Assert.AreEqual(expectedJsonSchema, actualJsonSchema);
        }


        private static IEnumerable<Type> GetClassesNamesToGenerateJSchema()
        {
            yield return typeof(SimpleStringFields.SimpleStringWithJsonPropertyName);
            yield return typeof(SimpleStringFields.SimpleStringWithMinimumLengthRestriction);
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