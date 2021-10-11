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
            var jSchemaForType = JSONSchemaGenerator.GetJSONSchema(typeof(MeetingSync).FullName).ToString();
            Console.WriteLine();
        }
    }
}