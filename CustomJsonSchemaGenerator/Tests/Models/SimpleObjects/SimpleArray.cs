// using System.Collections.Generic;
// using CustomJsonSchemaGenerator.Generator.CustomAttributes;
//
// namespace CustomJsonSchemaGenerator.Tests.Models.SimpleObjects
// {
//     public class SimpleArray
//     {
//         public class SimpleArrayWithDisallowAdditionalItemsAttribute
//         {
//             [DisallowAdditionalItems]
//             public IEnumerable<TestType> SimpleArrayField;
//
//             [DisallowAdditionalItems]
//             public IEnumerable<TestType> SimpleArrayProperty { get; set; }
//         }
//
//         public class SimpleArrayWithArrayItemsCannotBeNullAttribute
//         {
//             [ArrayItemsCannotBeNull]
//             public IEnumerable<TestType> SimpleArrayField;
//
//             [ArrayItemsCannotBeNull]
//             public IEnumerable<TestType> SimpleArrayProperty { get; set; }
//         }
//
//         public class TestType { }
//     }
// }