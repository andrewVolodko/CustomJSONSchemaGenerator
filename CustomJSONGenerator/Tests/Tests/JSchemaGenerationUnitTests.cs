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

        // [Test]
        // [TestCaseSource(nameof(GetSimpleStringClassesNamesToGenerateJSchema))]
        // public void VerifySimpleString(Type classToGenerateJSchema)
        // {
        //     AssertClassSchemas(classToGenerateJSchema, nameof(SimpleString));
        // }

        [Test]
        // [TestCaseSource(nameof(GetSimpleNumberClassesNamesToGenerateJSchema))]
        public void VerifySimpleNumber(Type classToGenerateJSchema)
        {
            AssertClassSchemas(classToGenerateJSchema, nameof(SimpleNumber));
        }

        // [Test]
        // [TestCaseSource(nameof(GetSimpleIntegerClassesNamesToGenerateJSchema))]
        // public void VerifySimpleInteger(Type classToGenerateJSchema)
        // {
        //     AssertClassSchemas(classToGenerateJSchema, nameof(SimpleInteger));
        // }
        //
        // [Test]
        // [TestCaseSource(nameof(GetSimpleArrayClassesNamesToGenerateJSchema))]
        // public void VerifySimpleArray(Type classToGenerateJSchema)
        // {
        //     AssertClassSchemas(classToGenerateJSchema, nameof(SimpleInteger));
        // }


        private static void AssertClassSchemas(Type classToGenerateJSchema, string folderName)
        {
            var expectedJsonSchemaFilePath =
                $"{ExpectedJsonSchemasFolderName}/{folderName}/{classToGenerateJSchema.Name}.json";

            var expectedJsonSchema = GetJsonObjectFromJsonFile(expectedJsonSchemaFilePath).ToString();
            var actualJsonSchema = GetJsonSchema(classToGenerateJSchema.FullName).ToString();

            Assert.AreEqual(expectedJsonSchema, actualJsonSchema);
        }

        // private static IEnumerable<Type> GetSimpleStringClassesNamesToGenerateJSchema()
        // {
        //     yield return typeof(SimpleString.SimpleStringWithJsonPropertyName);
        //     yield return typeof(SimpleString.SimpleStringWithRequiredAlways);
        //     yield return typeof(SimpleString.SimpleStringWithRequiredDefault);
        //     yield return typeof(SimpleString.SimpleStringWithRequiredAllowNull);
        //     yield return typeof(SimpleString.SimpleStringWithRequiredDisallowNull);
        //     yield return typeof(SimpleString.SimpleStringWithMinimumLength);
        //     yield return typeof(SimpleString.SimpleStringWithMaximumLength);
        //     yield return typeof(SimpleString.SimpleStringFieldWithMaximumLength);
        //     yield return typeof(SimpleString.SimpleStringWithRegEx);
        //     yield return typeof(SimpleString.SimpleStringWithFormat);
        // }

        // private static IEnumerable<Type> GetSimpleNumberClassesNamesToGenerateJSchema()
        // {
        //
        //     yield return typeof(SimpleNumber.SimpleNumberWithJsonPropertyName);
        //     yield return typeof(SimpleNumber.SimpleNumberWithRequiredAlways);
        //     yield return typeof(SimpleNumber.SimpleNumberWithRequiredDefault);
        //     yield return typeof(SimpleNumber.SimpleNumbersWithMinimum);
        //     yield return typeof(SimpleNumber.SimpleNumbersWithExclusiveMinimum);
        //     yield return typeof(SimpleNumber.SimpleNumbersWithMaximum);
        //     yield return typeof(SimpleNumber.SimpleNumbersWithExclusiveMaximum);
        //     yield return typeof(SimpleNumber.SimpleNumberWithMultipleOf);
        // }
        //
        // private static IEnumerable<Type> GetSimpleIntegerClassesNamesToGenerateJSchema()
        // {
        //     yield return typeof(SimpleInteger.SimpleIntegerWithJsonPropertyName);
        //     yield return typeof(SimpleInteger.SimpleIntegerWithRequiredAlways);
        //     yield return typeof(SimpleInteger.SimpleIntegerWithRequiredDefault);
        //     yield return typeof(SimpleInteger.SimpleIntegersWithMinimum);
        //     yield return typeof(SimpleInteger.SimpleIntegersWithExclusiveMinimum);
        //     yield return typeof(SimpleInteger.SimpleIntegersWithMaximum);
        //     yield return typeof(SimpleInteger.SimpleIntegersWithExclusiveMaximum);
        //     yield return typeof(SimpleInteger.SimpleIntegerWithMultipleOf);
        // }

        // private static IEnumerable<Type> GetSimpleArrayClassesNamesToGenerateJSchema()
        // {
        //     yield return typeof(SimpleArray.SimpleArrayWithJsonPropertyName);
        //     yield return typeof(SimpleArray.SimpleArrayWithRequiredAlways);
        //     yield return typeof(SimpleArray.SimpleArrayWithRequiredDefault);
        //     yield return typeof(SimpleArray.SimpleArrayWithRequiredAllowNull);
        // }

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