namespace ValueTuples
{
    public class Student : Person, ITypedRecord, IDeepCloneable<Student>
    {
        new ValueTuple<int, int> _value;

        public new ValueTuple<ValueTuple<int, string, string>, ValueTuple<int, int>> Value => ValueTuple.Create(base._value, _value);

        ITuple IRecord.Value { get { return Value; } set { base._value = ((ValueTuple<ValueTuple<int, string, string>, ValueTuple<int, int>>)value).Item1; _value = ((ValueTuple<ValueTuple<int, string, string>, ValueTuple<int, int>>)value).Item2; } }
        IRecordInfo ITypedRecord.GetInfo() => StudentInfo.Instance;

        Student IDeepCloneable<Student>.Clone() => new Student(Value.DeepClone());

        public Student() { }
        public Student(int id, string name, string address, int grade, int room) : base(id, name, address) { Grade = grade; Room = room; }
        public Student(ValueTuple<ValueTuple<int, string, string>, ValueTuple<int, int>> value) { _value = value.Item2; }

        public int Grade { get { return _value.Item1; } set { _value.Item1 = value; } }
        public int Room { get { return _value.Item2; } set { _value.Item2 = value; } }
    }

    internal class StudentInfo : IRecordInfo
    {
        public static readonly IRecordInfo Instance = new StudentInfo();

        private static readonly string[] _keys = new[] { "id", "name", "address", "grade", "room" };

        string IRecordInfo.GetKey(int index) => _keys[index];

        int IRecordInfo.GetIndex(string key)
        {
            switch (key)
            {
                default: return -1;
                case "id": return 0;
                case "name": return 1;
                case "address": return 2;
                case "grade": return 3;
                case "room": return 4;
            }
        }

        object IRecordInfo.NewInstance(int index, int? discriminator) => null;

        int? IRecordInfo.Discriminator => (int)PersonType.Student;
    }
}
