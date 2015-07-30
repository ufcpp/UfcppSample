using System.Collections.Generic;

namespace ValueTuples
{
    public class Person : IRecord, IDeepCloneable<Person>
    {
        protected ValueTuple<int, string, string> _value;

        public ValueTuple<int, string, string> Value => _value;

        ITuple IRecord.Value { get { return _value; } set { _value = (ValueTuple<int, string, string>)value; } }

        private static readonly string[] _keys = new[] { "id", "name", "address" };

        string IRecord.GetKey(int index) => _keys[index];

        int IRecord.GetIndex(string key)
        {
            switch (key)
            {
                default: return -1;
                case "id": return 0;
                case "name": return 1;
                case "address": return 2;
            }
        }

        int? IRecord.Discriminator => null;

        Person IDeepCloneable<Person>.Clone() => new Person(Value.DeepClone());

        public Person() { }
        public Person(int id, string name, string address) { Id = id; Name = name; Address = address; }
        public Person(ValueTuple<int, string, string> value) { _value = value; }

        public int Id { get { return _value.Item1; } set { _value.Item1 = value; } }
        public string Name { get { return _value.Item2; } set { _value.Item2 = value; } }
        public string Address { get { return _value.Item3; } set { _value.Item3 = value; } }
    }

    public enum PersonType
    {
        Student = 1,
    }

    public class Student : Person, IRecord, IDeepCloneable<Student>
    {
        new ValueTuple<int, int> _value;

        public new ValueTuple<ValueTuple<int, string, string>, ValueTuple<int, int>> Value => ValueTuple.Create(base._value, _value);

        ITuple IRecord.Value { get { return Value; } set { base._value = ((ValueTuple<ValueTuple<int, string, string>, ValueTuple<int, int>>)value).Item1; _value = ((ValueTuple<ValueTuple<int, string, string>, ValueTuple<int, int>>)value).Item2; } }

        private static readonly string[] _keys = new[] { "id", "name", "address", "grade", "room" };

        string IRecord.GetKey(int index) => _keys[index];

        int IRecord.GetIndex(string key)
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

        int? IRecord.Discriminator => (int)PersonType.Student;

        Student IDeepCloneable<Student>.Clone() => new Student(Value.DeepClone());

        public Student() { }
        public Student(int id, string name, string address, int grade, int room) : base(id, name, address) { Grade = grade; Room = room; }
        public Student(ValueTuple<ValueTuple<int, string, string>, ValueTuple<int, int>> value) { _value = value.Item2; }

        public int Grade { get { return _value.Item1; } set { _value.Item1 = value; } }
        public int Room { get { return _value.Item2; } set { _value.Item2 = value; } }
    }
}
