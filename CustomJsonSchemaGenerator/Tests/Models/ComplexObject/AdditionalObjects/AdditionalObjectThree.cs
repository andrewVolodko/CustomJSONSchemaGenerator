using System.Collections.Generic;

namespace CustomJsonSchemaGenerator.Tests.Models.ComplexObject.AdditionalObjects
{
    public class AdditionalObjectThree
    {
        public string AdditionalObjectThreeSimpleStringField;
        public string AdditionalObjectThreeSimpleStringProperty { get; set; }
        public double AdditionalObjectThreeSimpleNumberField;
        public double AdditionalObjectThreeSimpleNumberProperty { get; set; }
        public int AdditionalObjectThreeSimpleIntegerField;
        public int AdditionalObjectThreeSimpleIntegerProperty { get; set; }
        public List<string> AdditionalObjectThreeSimpleStringArrayField;
        public List<string> AdditionalObjectThreeSimpleStringArrayProperty { get; set; }
        public List<double> AdditionalObjectThreeSimpleNumberArrayField;
        public List<double> AdditionalObjectThreeSimpleNumberArrayProperty { get; set; }
        public List<int> AdditionalObjectThreeSimpleIntegerArrayField;
        public List<int> AdditionalObjectThreeSimpleIntegerArrayProperty { get; set; }
        public List<object> AdditionalObjectThreeSimpleObjectArrayField;
        public List<object> AdditionalObjectThreeSimpleObjectArrayProperty { get; set; }
    }
}