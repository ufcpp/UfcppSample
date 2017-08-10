namespace PropertyAccessor.SampleData
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public byte Code { get; set; }
        public bool TestCaseForLongPropertyName { get; set; }
        public bool TestCaseForMultiByteCharacterマルチバイト文字 { get; set; }

        public Item() { }
        public Item(int id, string name, double value, byte code, bool a, bool b)
        {
            Id = id;
            Name = name;
            Value = value;
            Code = code;
            TestCaseForLongPropertyName = a;
            TestCaseForMultiByteCharacterマルチバイト文字 = b;
        }

        public static bool Equals(Item x, Item y)
        {
            if (x == y) return true;
            if (x == null || y == null) return false;
            return x.Id == y.Id
                && x.Name == y.Name
                && x.Name == y.Name
                && x.Value == y.Value
                && x.Code == y.Code
                && x.TestCaseForLongPropertyName == y.TestCaseForLongPropertyName
                && x.TestCaseForMultiByteCharacterマルチバイト文字 == y.TestCaseForMultiByteCharacterマルチバイト文字
                ;
        }
    }
}
