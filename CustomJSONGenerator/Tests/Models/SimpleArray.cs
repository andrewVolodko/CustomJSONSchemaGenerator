// using System;
// using System.Collections.Generic;
// using CustomJSONGenerator.Generator;
// using Newtonsoft.Json;
//
// namespace CustomJSONGenerator.Tests.Models
// {
//     public class SimpleArray
//     {
//         [GenerateJsonSchema]
//         public class SimpleArrayWithJsonPropertyName
//         {
//             [JsonProperty("simpleArrayName")]
//             public IEnumerable<TestType> SimpleArray;
//         }
//
//         [GenerateJsonSchema]
//         public class SimpleArrayWithRequiredAlways
//         {
//             [JsonProperty(Required = Required.Always)]
//             public IEnumerable<TestType> SimpleArray;
//         }
//
//         [GenerateJsonSchema]
//         public class SimpleArrayWithRequiredDefault
//         {
//             [JsonProperty(Required = Required.Default)]
//             public IEnumerable<TestType> SimpleArray;
//         }
//
//         [GenerateJsonSchema]
//         public class SimpleArrayWithRequiredAllowNull
//         {
//             [JsonProperty(Required = Required.AllowNull)]
//             public IEnumerable<TestType> SimpleArray;
//         }
//
//         [GenerateJsonSchema]
//         public class SimpleArrayWithRequiredDisallowNull
//         {
//             [JsonProperty(Required = Required.DisallowNull)]
//             public IEnumerable<TestType> SimpleArray;
//         }
//
//         [GenerateJsonSchema]
//         public class SimpleArrayWithMinimumItems
//         {
//             [MinimumItems(1)]
//             public IEnumerable<TestType> SimpleArray;
//         }
//
//         [GenerateJsonSchema]
//         public class SimpleArrayWithMaximumItems
//         {
//             [MaximumItems(long.MaxValue)]
//             public IEnumerable<TestType> SimpleArray;
//         }
//
//         [GenerateJsonSchema]
//         public class SimpleArrayWithDisallowAdditionalItems
//         {
//             [DisallowAdditionalItems]
//             public IEnumerable<TestType> SimpleArray;
//         }
//
//         public class TestType { }
//     }
// }