using System.Collections.Generic;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;

namespace CustomJsonSchemaGenerator.Tests.Models
{
    public class SimpleArray
    {
        [GenerateJsonSchema]
        public class SimpleArrayWithDisallowAdditionalItemsAttribute
        {
            [DisallowAdditionalItems]
            public IEnumerable<TestType> SimpleArrayField;

            [DisallowAdditionalItems]
            public IEnumerable<TestType> SimpleArrayProperty { get; set; }
        }

        public class TestType { }
    }
}