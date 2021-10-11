using System;
using CustomJSONGenerator;
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
            var type = JSONSchemaGenerator.BuildTypeWithTypesToGenerateJSchema();

            var jSchemaFromType = JSONSchemaGenerator.GenerateJSchema(type);
            var test = jSchemaFromType.Properties[typeof(MeetingSync).FullName].ToString();
            Console.WriteLine();
        }
    }
}