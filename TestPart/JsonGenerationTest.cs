using System;
using CustomJSONGenerator;
using NUnit.Framework;

namespace TestPart
{
    [TestFixture]
    public class JsonGenerationTest
    {
        [Test]
        public void Test1()
        {
            var type = JSONSchemaGenerator.BuildTypeWithTypesToGenerateJSchema();

            var jSchemaFromType = JSONSchemaGenerator.GenerateJSchema(type).ToString();
            Console.WriteLine();
        }
    }
}