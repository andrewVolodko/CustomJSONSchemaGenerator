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
            yield return typeof(SimpleStringFields.SimpleStringWithJsonPropertyName);
            yield return typeof(SimpleStringFields.SimpleStringWithRequiredAlways);
            yield return typeof(SimpleStringFields.SimpleStringWithRequiredDefault);
            yield return typeof(SimpleStringFields.SimpleStringWithRequiredAllowNull);
            yield return typeof(SimpleStringFields.SimpleStringWithRequiredDisallowNull);
            yield return typeof(SimpleStringFields.SimpleStringWithMinimumLength);
            yield return typeof(SimpleStringFields.SimpleStringWithMaximumLength);
            yield return typeof(SimpleStringFields.SimpleStringFieldWithMaximumLength);
            yield return typeof(SimpleStringFields.SimpleStringWithRegEx);
            yield return typeof(SimpleStringFields.SimpleStringWithFormat);
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