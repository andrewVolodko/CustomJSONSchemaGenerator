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
    [MinimumProperties(14)]
    [MaximumProperties(42)]
    public class ThirdLevel : SecondLevel
    {
        [JsonProperty("thirdLevelSimpleStringField", Required = Required.Always)]
        [Format("TestFormatForField")]
        [MinLength(1), MaxLength(100)]
        [RegularExpression("TestPatternForField")]
        [EnumDataType(typeof(AdditionalEnum))]
        public string ThirdLevelSimpleStringField;

        [JsonProperty("thirdLevelSimpleStringProperty", Required = Required.Default)]
        [Format("TestFormatForProperty")]
        [MinLength(1), MaxLength(100)]
        [RegularExpression("TestPatternForProperty")]
        [EnumDataType(typeof(AdditionalEnum))]
        public string ThirdLevelSimpleStringProperty { get; set; }

        [JsonProperty("thirdLevelSimpleNumberField", Required = Required.AllowNull)]
        [Range(-100, 100)]
        [ExclusiveMaximum, ExclusiveMinimum]
        [MultipleOf(25.6)]
        public double ThirdLevelSimpleNumberField;

        [JsonProperty("thirdLevelSimpleNumberProperty", Required = Required.DisallowNull)]
        [Range(-100, 100)]
        [ExclusiveMaximum, ExclusiveMinimum]
        [MultipleOf(25.6)]
        public double ThirdLevelSimpleNumberProperty { get; set; }

        [JsonProperty("thirdLevelSimpleIntegerField", Required = Required.Always)]
        [Range(-100, 100)]
        [ExclusiveMaximum, ExclusiveMinimum]
        [MultipleOf(25)]
        public int ThirdLevelSimpleIntegerField;

        [JsonProperty("thirdLevelSimpleIntegerProperty", Required = Required.Default)]
        [Range(-100, 100)]
        [ExclusiveMaximum, ExclusiveMinimum]
        [MultipleOf(25)]
        public int ThirdLevelSimpleIntegerProperty { get; set; }

        [JsonProperty("thirdLevelSimpleStringArrayField", Required = Required.AllowNull)]
        [ArrayItemsCannotBeNull]
        [UniqueItems]
        [Contains(JSchemaType.String)]
        [MinContains(1), MaxContains(1000)]
        [MinLength(1), MaxLength(1000)]
        [DisallowAdditionalItems]
        public List<string> ThirdLevelSimpleStringArrayField;

        [JsonProperty("thirdLevelSimpleStringArrayProperty", Required = Required.DisallowNull)]
        [ArrayItemsCannotBeNull]
        [UniqueItems]
        [Contains(JSchemaType.String)]
        [MinContains(1), MaxContains(1000)]
        [MinLength(1), MaxLength(1000)]
        [DisallowAdditionalItems]
        public List<string> ThirdLevelSimpleStringArrayProperty { get; set; }

        [JsonProperty("thirdLevelSimpleNumberArrayField", Required = Required.Always)]
        [ArrayItemsCannotBeNull]
        [UniqueItems]
        [Contains(JSchemaType.Number)]
        [MinContains(1), MaxContains(1000)]
        [MinLength(1), MaxLength(1000)]
        [DisallowAdditionalItems]
        public List<double> ThirdLevelSimpleNumberArrayField;

        [JsonProperty("thirdLevelSimpleNumberArrayProperty", Required = Required.Default)]
        [ArrayItemsCannotBeNull]
        [UniqueItems]
        [Contains(JSchemaType.Number)]
        [MinContains(1), MaxContains(1000)]
        [MinLength(1), MaxLength(1000)]
        [DisallowAdditionalItems]
        public List<double> ThirdLevelSimpleNumberArrayProperty { get; set; }

        [JsonProperty("thirdLevelSimpleIntegerArrayField", Required = Required.AllowNull)]
        [ArrayItemsCannotBeNull]
        [UniqueItems]
        [Contains(JSchemaType.Integer)]
        [MinContains(1), MaxContains(1000)]
        [MinLength(1), MaxLength(1000)]
        [DisallowAdditionalItems]
        public List<int> ThirdLevelSimpleIntegerArrayField;

        [JsonProperty("thirdLevelSimpleIntegerArrayProperty", Required = Required.DisallowNull)]
        [ArrayItemsCannotBeNull]
        [UniqueItems]
        [Contains(JSchemaType.Integer)]
        [MinContains(1), MaxContains(1000)]
        [MinLength(1), MaxLength(1000)]
        [DisallowAdditionalItems]
        public List<int> ThirdLevelSimpleIntegerArrayProperty { get; set; }

        [JsonProperty("thirdLevelSimpleObjectArrayField", Required = Required.Always)]
        [ArrayItemsCannotBeNull]
        [UniqueItems]
        [Contains(JSchemaType.Object)]
        [MinContains(1), MaxContains(1000)]
        [MinLength(1), MaxLength(1000)]
        [DisallowAdditionalItems]
        public List<AdditionalObjectThree> ThirdLevelSimpleObjectArrayField;

        [JsonProperty("thirdLevelSimpleObjectArrayProperty", Required = Required.Always)]
        [ArrayItemsCannotBeNull]
        [UniqueItems]
        [Contains(JSchemaType.Object)]
        [MinContains(1), MaxContains(1000)]
        [MinLength(1), MaxLength(1000)]
        [DisallowAdditionalItems]
        public List<AdditionalObjectThree> ThirdLevelSimpleObjectArrayProperty { get; set; }
    }
}