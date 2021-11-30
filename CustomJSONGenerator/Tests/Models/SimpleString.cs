// using CustomJSONGenerator.Generator;
// using Newtonsoft.Json;
// using Required = Newtonsoft.Json.Required;
//
// namespace CustomJSONGenerator.Tests.Models
// {
//     public class SimpleString
//     {
//         [GenerateJsonSchema]
//         public class SimpleStringWithJsonPropertyName
//         {
//             [JsonProperty("simpleStringName")]
//             public string SimpleString;
//         }
//
//         [GenerateJsonSchema]
//         public class SimpleStringWithRequiredAlways
//         {
//             [JsonProperty(Required = Required.Always)]
//             public string SimpleString;
//         }
//
//         [GenerateJsonSchema]
//         public class SimpleStringWithRequiredDefault
//         {
//             [JsonProperty(Required = Required.Default)]
//             public string SimpleString;
//         }
//
//         [GenerateJsonSchema]
//         public class SimpleStringWithRequiredAllowNull
//         {
//             [JsonProperty(Required = Required.AllowNull)]
//             public string SimpleString;
//         }
//
//         [GenerateJsonSchema]
//         public class SimpleStringWithRequiredDisallowNull
//         {
//             [JsonProperty(Required = Required.DisallowNull)]
//             public string SimpleString;
//         }
//
//         [GenerateJsonSchema]
//         public class SimpleStringWithMinimumLength
//         {
//             [MinimumLength(1)]
//             public string SimpleString;
//         }
//
//         [GenerateJsonSchema]
//         public class SimpleStringWithMaximumLength
//         {
//             [MaximumLength(long.MaxValue)]
//             public string SimpleString { get; set; }
//         }
//
//         [GenerateJsonSchema]
//         public class SimpleStringFieldWithMaximumLength
//         {
//             [MaximumLength(long.MaxValue)]
//             public string SimpleString;
//         }
//
//         [GenerateJsonSchema]
//         public class SimpleStringWithRegEx
//         {
//             [RegEx("^.+(@mail\\.ru)$)")]
//             public string SimpleString;
//         }
//
//         [GenerateJsonSchema]
//         public class SimpleStringWithFormat
//         {
//             [StringFormat("uuid")]
//             public string SimpleString;
//         }
//     }
// }