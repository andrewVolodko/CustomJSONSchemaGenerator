// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using CustomJsonSchemaGenerator.Generator.CustomAttributes;
// using Newtonsoft.Json;
// using Newtonsoft.Json.Schema;
//
// namespace CustomJsonSchemaGenerator.Tests.Models.ComplexObject.AdditionalObjects
// {
//     public class AdditionalObjectOne
//     {
//         [JsonProperty("additionalObjectOneSimpleStringField", Required = Required.Always)]
//         [Format("TestFormatForField")]
//         [MinLength(1), MaxLength(100)]
//         [RegularExpression("TestPatternForField")]
//         public string AdditionalObjectOneSimpleStringField;
//
//         [JsonProperty("additionalObjectOneSimpleStringProperty", Required = Required.Default)]
//         [Format("TestFormatForProperty")]
//         [MinLength(1), MaxLength(100)]
//         [RegularExpression("TestPatternForProperty")]
//         public string AdditionalObjectOneSimpleStringProperty { get; set; }
//
//         [JsonProperty("additionalObjectOneSimpleNumberField", Required = Required.AllowNull)]
//         [Range(-100, 100)]
//         [ExclusiveMaximum, ExclusiveMinimum]
//         [MultipleOf(25.6)]
//         public double AdditionalObjectOneSimpleNumberField;
//
//         [JsonProperty("additionalObjectOneSimpleNumberProperty", Required = Required.DisallowNull)]
//         [Range(-100, 100)]
//         [ExclusiveMaximum, ExclusiveMinimum]
//         [MultipleOf(25.6)]
//         public double AdditionalObjectOneSimpleNumberProperty { get; set; }
//
//         [JsonProperty("additionalObjectOneSimpleIntegerField", Required = Required.Always)]
//         [Range(-100, 100)]
//         [ExclusiveMaximum, ExclusiveMinimum]
//         [MultipleOf(25)]
//         public int AdditionalObjectOneSimpleIntegerField;
//
//         [JsonProperty("additionalObjectOneSimpleIntegerProperty", Required = Required.Default)]
//         [Range(-100, 100)]
//         [ExclusiveMaximum, ExclusiveMinimum]
//         [MultipleOf(25)]
//         public int AdditionalObjectOneSimpleIntegerProperty { get; set; }
//
//         [JsonProperty("additionalObjectOneSimpleStringArrayField", Required = Required.AllowNull)]
//         [ArrayItemsCannotBeNull]
//         [UniqueItems]
//         [Contains(JSchemaType.String)]
//         [MinContains(1), MaxContains(1000)]
//         [MinLength(1), MaxLength(1000)]
//         [DisallowAdditionalItems]
//         public List<string> AdditionalObjectOneSimpleStringArrayField;
//
//         [JsonProperty("additionalObjectOneSimpleStringArrayProperty", Required = Required.DisallowNull)]
//         [ArrayItemsCannotBeNull]
//         [UniqueItems]
//         [Contains(JSchemaType.String)]
//         [MinContains(1), MaxContains(1000)]
//         [MinLength(1), MaxLength(1000)]
//         [DisallowAdditionalItems]
//         public List<string> AdditionalObjectOneSimpleStringArrayProperty { get; set; }
//
//         [JsonProperty("additionalObjectOneSimpleNumberArrayField", Required = Required.Always)]
//         [ArrayItemsCannotBeNull]
//         [UniqueItems]
//         [Contains(JSchemaType.Number)]
//         [MinContains(1), MaxContains(1000)]
//         [MinLength(1), MaxLength(1000)]
//         [DisallowAdditionalItems]
//         public List<double> AdditionalObjectOneSimpleNumberArrayField;
//
//         [JsonProperty("additionalObjectOneSimpleNumberArrayProperty", Required = Required.Default)]
//         [ArrayItemsCannotBeNull]
//         [UniqueItems]
//         [Contains(JSchemaType.Number)]
//         [MinContains(1), MaxContains(1000)]
//         [MinLength(1), MaxLength(1000)]
//         [DisallowAdditionalItems]
//         public List<double> AdditionalObjectOneSimpleNumberArrayProperty { get; set; }
//
//         [JsonProperty("additionalObjectOneSimpleIntegerArrayField", Required = Required.AllowNull)]
//         [ArrayItemsCannotBeNull]
//         [UniqueItems]
//         [Contains(JSchemaType.Integer)]
//         [MinContains(1), MaxContains(1000)]
//         [MinLength(1), MaxLength(1000)]
//         [DisallowAdditionalItems]
//         public List<int> AdditionalObjectOneSimpleIntegerArrayField;
//
//         [JsonProperty("additionalObjectOneSimpleIntegerArrayProperty", Required = Required.DisallowNull)]
//         [ArrayItemsCannotBeNull]
//         [UniqueItems]
//         [Contains(JSchemaType.Integer)]
//         [MinContains(1), MaxContains(1000)]
//         [MinLength(1), MaxLength(1000)]
//         [DisallowAdditionalItems]
//         public List<int> AdditionalObjectOneSimpleIntegerArrayProperty { get; set; }
//
//         [JsonProperty("additionalObjectOneSimpleObjectArrayField", Required = Required.Always)]
//         [ArrayItemsCannotBeNull]
//         [UniqueItems]
//         [MinContains(1), MaxContains(1000)]
//         [MinLength(1), MaxLength(1000)]
//         [DisallowAdditionalItems]
//         public List<AdditionalObjectTwo> AdditionalObjectOneSimpleObjectArrayField;
//
//         [JsonProperty("additionalObjectOneSimpleObjectArrayProperty", Required = Required.Always)]
//         [ArrayItemsCannotBeNull]
//         [UniqueItems]
//         [MinContains(1), MaxContains(1000)]
//         [MinLength(1), MaxLength(1000)]
//         [DisallowAdditionalItems]
//         public List<AdditionalObjectTwo> AdditionalObjectOneSimpleObjectArrayProperty { get; set; }
//     }
// }