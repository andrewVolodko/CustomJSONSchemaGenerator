// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using CustomJsonSchemaGenerator.Generator.CustomAttributes;
// using CustomJsonSchemaGenerator.Tests.Models.ComplexObject.AdditionalObjects;
// using Newtonsoft.Json;
// using Newtonsoft.Json.Schema;
//
// namespace CustomJsonSchemaGenerator.Tests.Models.ComplexObject
// {
//     [MinimumProperties(14)]
//     [MaximumProperties(28)]
//     public class SecondLevel : FirstLevel
//     {
//         [JsonProperty("secondLevelSimpleStringField", Required = Required.Always)]
//         [Format("TestFormatForField")]
//         [MinLength(1), MaxLength(100)]
//         [RegularExpression("TestPatternForField")]
//         [EnumDataType(typeof(AdditionalEnum))]
//         public string SecondLevelSimpleStringField;
//
//         [JsonProperty("secondLevelSimpleStringProperty", Required = Required.Default)]
//         [Format("TestFormatForProperty")]
//         [MinLength(1), MaxLength(100)]
//         [RegularExpression("TestPatternForProperty")]
//         [EnumDataType(typeof(AdditionalEnum))]
//         public string SecondLevelSimpleStringProperty { get; set; }
//
//         [JsonProperty("secondLevelSimpleNumberField", Required = Required.AllowNull)]
//         [Range(-100, 100)]
//         [ExclusiveMaximum, ExclusiveMinimum]
//         [MultipleOf(25.6)]
//         public double SecondLevelSimpleNumberField;
//
//         [JsonProperty("secondLevelSimpleNumberProperty", Required = Required.DisallowNull)]
//         [Range(-100, 100)]
//         [ExclusiveMaximum, ExclusiveMinimum]
//         [MultipleOf(25.6)]
//         public double SecondLevelSimpleNumberProperty { get; set; }
//
//         [JsonProperty("secondLevelSimpleIntegerField", Required = Required.Always)]
//         [Range(-100, 100)]
//         [ExclusiveMaximum, ExclusiveMinimum]
//         [MultipleOf(25)]
//         public int SecondLevelSimpleIntegerField;
//
//         [JsonProperty("secondLevelSimpleIntegerProperty", Required = Required.Default)]
//         [Range(-100, 100)]
//         [ExclusiveMaximum, ExclusiveMinimum]
//         [MultipleOf(25)]
//         public int SecondLevelSimpleIntegerProperty { get; set; }
//
//         [JsonProperty("secondLevelSimpleStringArrayField", Required = Required.AllowNull)]
//         [ArrayItemsCannotBeNull]
//         [UniqueItems]
//         [Contains(JSchemaType.String)]
//         [MinContains(1), MaxContains(1000)]
//         [MinLength(1), MaxLength(1000)]
//         [DisallowAdditionalItems]
//         public List<string> SecondLevelSimpleStringArrayField;
//
//         [JsonProperty("secondLevelSimpleStringArrayProperty", Required = Required.DisallowNull)]
//         [ArrayItemsCannotBeNull]
//         [UniqueItems]
//         [Contains(JSchemaType.String)]
//         [MinContains(1), MaxContains(1000)]
//         [MinLength(1), MaxLength(1000)]
//         [DisallowAdditionalItems]
//         public List<string> SecondLevelSimpleStringArrayProperty { get; set; }
//
//         [JsonProperty("secondLevelSimpleNumberArrayField", Required = Required.Always)]
//         [ArrayItemsCannotBeNull]
//         [UniqueItems]
//         [Contains(JSchemaType.Number)]
//         [MinContains(1), MaxContains(1000)]
//         [MinLength(1), MaxLength(1000)]
//         [DisallowAdditionalItems]
//         public List<double> SecondLevelSimpleNumberArrayField;
//
//         [JsonProperty("secondLevelSimpleNumberArrayProperty", Required = Required.Default)]
//         [ArrayItemsCannotBeNull]
//         [UniqueItems]
//         [Contains(JSchemaType.Number)]
//         [MinContains(1), MaxContains(1000)]
//         [MinLength(1), MaxLength(1000)]
//         [DisallowAdditionalItems]
//         public List<double> SecondLevelSimpleNumberArrayProperty { get; set; }
//
//         [JsonProperty("secondLevelSimpleIntegerArrayField", Required = Required.AllowNull)]
//         [ArrayItemsCannotBeNull]
//         [UniqueItems]
//         [Contains(JSchemaType.Integer)]
//         [MinContains(1), MaxContains(1000)]
//         [MinLength(1), MaxLength(1000)]
//         [DisallowAdditionalItems]
//         public List<int> SecondLevelSimpleIntegerArrayField;
//
//         [JsonProperty("secondLevelSimpleIntegerArrayProperty", Required = Required.DisallowNull)]
//         [ArrayItemsCannotBeNull]
//         [UniqueItems]
//         [Contains(JSchemaType.Integer)]
//         [MinContains(1), MaxContains(1000)]
//         [MinLength(1), MaxLength(1000)]
//         [DisallowAdditionalItems]
//         public List<int> SecondLevelSimpleIntegerArrayProperty { get; set; }
//
//         [JsonProperty("secondLevelSimpleObjectArrayField", Required = Required.Always)]
//         [ArrayItemsCannotBeNull]
//         [UniqueItems]
//         [Contains(JSchemaType.Object)]
//         [MinContains(1), MaxContains(1000)]
//         [MinLength(1), MaxLength(1000)]
//         [DisallowAdditionalItems]
//         public List<AdditionalObjectTwo> SecondLevelSimpleObjectArrayField;
//
//         [JsonProperty("secondLevelSimpleObjectArrayProperty", Required = Required.Always)]
//         [ArrayItemsCannotBeNull]
//         [UniqueItems]
//         [Contains(JSchemaType.Object)]
//         [MinContains(1), MaxContains(1000)]
//         [MinLength(1), MaxLength(1000)]
//         [DisallowAdditionalItems]
//         public List<AdditionalObjectTwo> SecondLevelSimpleObjectArrayProperty { get; set; }
//     }
// }