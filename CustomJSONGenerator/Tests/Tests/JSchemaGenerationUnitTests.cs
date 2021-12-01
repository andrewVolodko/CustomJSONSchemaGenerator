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
        public void VerifySimpleString(Type classToGenerateJSchema)
        {
            AssertClassSchemas(classToGenerateJSchema, nameof(SimpleString));
        }

        [Test]
        [TestCaseSource(nameof(GetSimpleIntegerClassesNamesToGenerateJSchema))]
        public void VerifySimpleInteger(Type classToGenerateJSchema)
        {
            AssertClassSchemas(classToGenerateJSchema, nameof(SimpleInteger));
        }

        [Test]
        [TestCaseSource(nameof(GetSimpleNumberClassesNamesToGenerateJSchema))]
        public void VerifySimpleNumber(Type classToGenerateJSchema)
        {
            AssertClassSchemas(classToGenerateJSchema, nameof(SimpleNumber));
        }

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

        private static IEnumerable<Type> GetSimpleStringClassesNamesToGenerateJSchema()
        {
            yield return typeof(SimpleString.SimpleStringWithFormat);
        }

        private static IEnumerable<Type> GetSimpleIntegerClassesNamesToGenerateJSchema()
        {
            yield return typeof(SimpleInteger.SimpleIntegerWithExclusiveMinimum);
            yield return typeof(SimpleInteger.SimpleIntegerWithExclusiveMaximum);
            yield return typeof(SimpleInteger.SimpleIntegerWithExclusiveMinimumAndExclusiveMaximum);
            yield return typeof(SimpleInteger.SimpleIntegerWithMultipleOf);
        }

        private static IEnumerable<Type> GetSimpleNumberClassesNamesToGenerateJSchema()
        {
            yield return typeof(SimpleNumber.SimpleNumberWithExclusiveMinimum);
            yield return typeof(SimpleNumber.SimpleNumberWithExclusiveMaximum);
            yield return typeof(SimpleNumber.SimpleNumberWithExclusiveMinimumAndExclusiveMaximum);
            yield return typeof(SimpleNumber.SimpleNumberWithMultipleOf);
        }


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