namespace ValueTuples
{
    public class Person : ITypedRecord, IDeepCloneable<Person>
    {
        protected ValueTuple<int, string, string> _value;

        public ValueTuple<int, string, string> Value => _value;

        ITuple IRecord.Value { get { return _value; } set { _value = (ValueTuple<int, string, string>)value; } }
        IRecordInfo ITypedRecord.GetInfo() => PersonInfo.Instance;

        Person IDeepCloneable<Person>.Clone() => new Person(Value.DeepClone());

        public Person() { }
        public Person(int id, string name, string address) { Id = id; Name = name; Address = address; }
        public Person(ValueTuple<int, string, string> value) { _value = value; }

        public int Id { get { return _value.Item1; } set { _value.Item1 = value; } }
        public string Name { get { return _value.Item2; } set { _value.Item2 = value; } }
        public string Address { get { return _value.Item3; } set { _value.Item3 = value; } }
    }

    internal class PersonInfo : IRecordInfo
    {
        public static readonly IRecordInfo Instance = new PersonInfo();

        private static readonly string[] _keys = new[] { "id", "name", "address" };

        string IRecordInfo.GetKey(int index) => _keys[index];

        int IRecordInfo.GetIndex(string key)
        {
            switch (key)
            {
                default: return -1;
                case "id": return 0;
                case "name": return 1;
                case "address": return 2;
            }
        }

        object IRecordInfo.NewInstance(int index, int? discriminator) => null;

        int? IRecordInfo.Discriminator => null;
    }

    public enum PersonType
    {
        Student = 1,
    }
}
