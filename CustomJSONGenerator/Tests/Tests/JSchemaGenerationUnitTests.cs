using System;
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
        [TestCaseSource(nameof(GetSimpleStringClassesNamesToGenerateJSchema))]
        public void VerifySimpleStringWithJsonPropertyName(Type classToGenerateJSchema)
        {
            var expectedJsonSchema = GetJsonObjectFromJsonFile($"{ExpectedJsonSchemasFolderName}/SimpleString/{classToGenerateJSchema.Name}.json").ToString();
            var actualJsonSchema = GetJsonSchema(classToGenerateJSchema.FullName).ToString();

            Assert.AreEqual(expectedJsonSchema, actualJsonSchema);
        }


        private static IEnumerable<Type> GetSimpleStringClassesNamesToGenerateJSchema()
        {
            yield return typeof(SimpleString.SimpleStringWithJsonPropertyName);
            yield return typeof(SimpleString.SimpleStringWithRequiredAlways);
            yield return typeof(SimpleString.SimpleStringWithRequiredDefault);
            yield return typeof(SimpleString.SimpleStringWithRequiredAllowNull);
            yield return typeof(SimpleString.SimpleStringWithRequiredDisallowNull);
            yield return typeof(SimpleString.SimpleStringWithMinimumLength);
            yield return typeof(SimpleString.SimpleStringWithMaximumLength);
            yield return typeof(SimpleString.SimpleStringFieldWithMaximumLength);
            yield return typeof(SimpleString.SimpleStringWithRegEx);
            yield return typeof(SimpleString.SimpleStringWithFormat);
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