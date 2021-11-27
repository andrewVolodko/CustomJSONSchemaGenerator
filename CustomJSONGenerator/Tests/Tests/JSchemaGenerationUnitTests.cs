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
            AssertClassSchemas(classToGenerateJSchema, nameof(SimpleString));
        }

        [Test]
        [TestCaseSource(nameof(GetSimpleNumberClassesNamesToGenerateJSchema))]
        public void VerifySimpleNumberWithJsonPropertyName(Type classToGenerateJSchema)
        {
            AssertClassSchemas(classToGenerateJSchema, nameof(SimpleNumber));
        }


        private static void AssertClassSchemas(Type classToGenerateJSchema, string folderName)
        {
            var expectedJsonSchema = GetJsonObjectFromJsonFile($"{ExpectedJsonSchemasFolderName}/{folderName}/{classToGenerateJSchema.Name}.json").ToString();
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

        private static IEnumerable<Type> GetSimpleNumberClassesNamesToGenerateJSchema()
        {
            yield return typeof(SimpleNumber.SimpleNumberWithJsonPropertyName);
            yield return typeof(SimpleNumber.SimpleNumberWithRequiredAlways);
            yield return typeof(SimpleNumber.SimpleNumberWithRequiredDefault);
            yield return typeof(SimpleNumber.SimpleNumberWithMinimum);
            yield return typeof(SimpleNumber.SimpleNumberWithExclusiveMinimum);
            yield return typeof(SimpleNumber.SimpleNumberWithMaximum);
            yield return typeof(SimpleNumber.SimpleNumberWithExclusiveMaximum);
            yield return typeof(SimpleNumber.SimpleNumberWithMultipleOf);
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