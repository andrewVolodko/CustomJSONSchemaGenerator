using System.Collections.Generic;
using CustomJSONGenerator.Generator;

namespace CustomJSONGenerator.Tests.Models
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