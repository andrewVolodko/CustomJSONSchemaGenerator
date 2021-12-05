using System.Collections.Generic;

namespace CustomJsonSchemaGenerator.Tests.Models.ComplexObject.AdditionalObjects
{
    public class AdditionalObjectTwo
    {
        public string AdditionalObjectTwoSimpleStringField;
        public string AdditionalObjectTwoSimpleStringProperty { get; set; }
        public double AdditionalObjectTwoSimpleNumberField;
        public double AdditionalObjectTwoSimpleNumberProperty { get; set; }
        public int AdditionalObjectTwoSimpleIntegerField;
        public int AdditionalObjectTwoSimpleIntegerProperty { get; set; }
        public List<string> AdditionalObjectTwoSimpleStringArrayField;
        public List<string> AdditionalObjectTwoSimpleStringArrayProperty { get; set; }
        public List<double> AdditionalObjectTwoSimpleNumberArrayField;
        public List<double> AdditionalObjectTwoSimpleNumberArrayProperty { get; set; }
        public List<int> AdditionalObjectTwoSimpleIntegerArrayField;
        public List<int> AdditionalObjectTwoSimpleIntegerArrayProperty { get; set; }
        public List<object> AdditionalObjectTwoSimpleObjectArrayField;
        public List<object> AdditionalObjectTwoSimpleObjectArrayProperty { get; set; }
    }
}