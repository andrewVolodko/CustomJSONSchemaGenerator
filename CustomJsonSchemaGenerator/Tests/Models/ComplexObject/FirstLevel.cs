using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using CustomJsonSchemaGenerator.Tests.Models.ComplexObject.AdditionalObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace CustomJsonSchemaGenerator.Tests.Models.ComplexObject
{
    [GenerateJsonSchema]
    [DisallowAdditionalProperties]
    public class FirstLevel
    {
        [JsonProperty("firstLevelSimpleStringField", Required = Required.Always)]
        [Format("TestFormatForField")]
        [MinLength(1), MaxLength(100)]
        [RegularExpression("TestPatternForField")]
        public string FirstLevelSimpleStringField;

        [JsonProperty("firstLevelSimpleStringProperty", Required = Required.Default)]
        [Format("TestFormatForProperty")]
        [MinLength(1), MaxLength(100)]
        [RegularExpression("TestPatternForProperty")]
        public string FirstLevelSimpleStringProperty { get; set; }

        [JsonProperty("firstLevelSimpleNumberField", Required = Required.AllowNull)]
        [Range(-100, 100)]
        [ExclusiveMaximum, ExclusiveMinimum]
        [MultipleOf(25.6)]
        public double FirstLevelSimpleNumberField;

        [JsonProperty("firstLevelSimpleNumberProperty", Required = Required.DisallowNull)]
        [Range(-100, 100)]
        [ExclusiveMaximum, ExclusiveMinimum]
        [MultipleOf(25.6)]
        public double FirstLevelSimpleNumberProperty { get; set; }

        [JsonProperty("firstLevelSimpleIntegerField", Required = Required.Always)]
        [Range(-100, 100)]
        [ExclusiveMaximum, ExclusiveMinimum]
        [MultipleOf(25)]
        public int FirstLevelSimpleIntegerField;

        [JsonProperty("firstLevelSimpleIntegerProperty", Required = Required.Default)]
        [Range(-100, 100)]
        [ExclusiveMaximum, ExclusiveMinimum]
        [MultipleOf(25)]
        public int FirstLevelSimpleIntegerProperty { get; set; }

        [JsonProperty("firstLevelSimpleStringArrayField", Required = Required.AllowNull)]
        [ArrayItemsCannotBeNull]
        [UniqueItems]
        [Contains(JSchemaType.String)]
        [MinContains(1), MaxContains(1000)]
        [MinLength(1), MaxLength(1000)]
        [DisallowAdditionalItems]
        public List<string> FirstLevelSimpleStringArrayField;

        [JsonProperty("firstLevelSimpleStringArrayProperty", Required = Required.DisallowNull)]
        [ArrayItemsCannotBeNull]
        [UniqueItems]
        [Contains(JSchemaType.String)]
        [MinContains(1), MaxContains(1000)]
        [MinLength(1), MaxLength(1000)]
        [DisallowAdditionalItems]
        public List<string> FirstLevelSimpleStringArrayProperty { get; set; }

        [JsonProperty("firstLevelSimpleNumberArrayField", Required = Required.Always)]
        [ArrayItemsCannotBeNull]
        [UniqueItems]
        [Contains(JSchemaType.Number)]
        [MinContains(1), MaxContains(1000)]
        [MinLength(1), MaxLength(1000)]
        [DisallowAdditionalItems]
        public List<double> FirstLevelSimpleNumberArrayField;

        [JsonProperty("firstLevelSimpleNumberArrayProperty", Required = Required.Default)]
        [ArrayItemsCannotBeNull]
        [UniqueItems]
        [Contains(JSchemaType.Number)]
        [MinContains(1), MaxContains(1000)]
        [MinLength(1), MaxLength(1000)]
        [DisallowAdditionalItems]
        public List<double> FirstLevelSimpleNumberArrayProperty { get; set; }

        [JsonProperty("firstLevelSimpleIntegerArrayField", Required = Required.AllowNull)]
        [ArrayItemsCannotBeNull]
        [UniqueItems]
        [Contains(JSchemaType.Integer)]
        [MinContains(1), MaxContains(1000)]
        [MinLength(1), MaxLength(1000)]
        [DisallowAdditionalItems]
        public List<int> FirstLevelSimpleIntegerArrayField;

        [JsonProperty("firstLevelSimpleIntegerArrayProperty", Required = Required.DisallowNull)]
        [ArrayItemsCannotBeNull]
        [UniqueItems]
        [Contains(JSchemaType.Integer)]
        [MinContains(1), MaxContains(1000)]
        [MinLength(1), MaxLength(1000)]
        [DisallowAdditionalItems]
        public List<int> FirstLevelSimpleIntegerArrayProperty { get; set; }

        [JsonProperty("firstLevelSimpleObjectArrayField", Required = Required.Always)]
        [ArrayItemsCannotBeNull]
        [UniqueItems]
        [Contains(JSchemaType.Object)]
        [MinContains(1), MaxContains(1000)]
        [MinLength(1), MaxLength(1000)]
        [DisallowAdditionalItems]
        public List<AdditionalObjectOne> FirstLevelSimpleObjectArrayField;

        [JsonProperty("firstLevelSimpleObjectArrayProperty", Required = Required.Always)]
        [ArrayItemsCannotBeNull]
        [UniqueItems]
        [Contains(JSchemaType.Object)]
        [MinContains(1), MaxContains(1000)]
        [MinLength(1), MaxLength(1000)]
        [DisallowAdditionalItems]
        public List<AdditionalObjectOne> FirstLevelSimpleObjectArrayProperty { get; set; }
    }
}