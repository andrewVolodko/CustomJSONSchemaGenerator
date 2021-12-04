using System;
using System.Collections.Generic;
using System.IO;
using CustomJsonSchemaGenerator.Tests.Models;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using static CustomJsonSchemaGenerator.Generator.CustomJSchemaGenerator;

namespace CustomJsonSchemaGenerator.Tests.Tests
{
    public class JSchemaGenerationUnitTests
    {
        private const string ExpectedJsonSchemasFolderName = "Tests/ExpectedJsonSchemas";
        private const string ExpectedSimpleJsonSchemasFolderName = "Simple";

        [Test]
        [TestCaseSource(nameof(GetSimpleStringClassesNamesToGenerateJSchema))]
        public void VerifySimpleString(Type classToGenerateJSchema)
        {
            AssertClassSchemas(classToGenerateJSchema, ExpectedSimpleJsonSchemasFolderName, nameof(SimpleString));
        }

        [Test]
        [TestCaseSource(nameof(GetSimpleIntegerClassesNamesToGenerateJSchema))]
        public void VerifySimpleInteger(Type classToGenerateJSchema)
        {
            AssertClassSchemas(classToGenerateJSchema, ExpectedSimpleJsonSchemasFolderName, nameof(SimpleInteger));
        }

        [Test]
        [TestCaseSource(nameof(GetSimpleNumberClassesNamesToGenerateJSchema))]
        public void VerifySimpleNumber(Type classToGenerateJSchema)
        {
            AssertClassSchemas(classToGenerateJSchema, ExpectedSimpleJsonSchemasFolderName, nameof(SimpleNumber));
        }

        [Test]
        [TestCaseSource(nameof(GetSimpleArrayClassesNamesToGenerateJSchema))]
        public void VerifySimpleArray(Type classToGenerateJSchema)
        {
            AssertClassSchemas(classToGenerateJSchema, ExpectedSimpleJsonSchemasFolderName, nameof(SimpleArray));
        }


        private static void AssertClassSchemas(Type classToGenerateJSchema, params string[] path)
        {
            var pathList = new List<string>();
            pathList.Add(ExpectedJsonSchemasFolderName);
            pathList.AddRange(path);
            pathList.Add(classToGenerateJSchema.Name + ".json");

            var expectedJsonSchemaFilePath = Path.Combine(pathList.ToArray());

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

        private static IEnumerable<Type> GetSimpleArrayClassesNamesToGenerateJSchema()
        {
            yield return typeof(SimpleArray.SimpleArrayWithDisallowAdditionalItemsAttribute);
            yield return typeof(SimpleArray.SimpleArrayWithArrayItemsCannotBeNullAttribute);
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