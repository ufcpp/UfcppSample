using System.Collections.Generic;

namespace ValueTuples
{
    public class Person
    {
        protected ValueTuple<int, string, string> _value;

        public ValueTuple<int, string, string> Value => _value;

        public static readonly IEnumerable<string> Keys = new[] { "id", "name", "address" };

        public static int GetIndex(string key)
        {
            switch (key)
            {
                default: return -1;
                case "id": return 0;
                case "name": return 1;
                case "address": return 2;
            }
        }

        public Person() { }
        public Person(int id, string name, string address) { Id = id; Name = name; Address = address; }
        public Person(ValueTuple<int, string, string> value) { _value = value; }

        public int Id { get { return _value.Item1; } set { _value.Item1 = value; } }
        public string Name { get { return _value.Item2; } set { _value.Item2 = value; } }
        public string Address { get { return _value.Item3; } set { _value.Item3 = value; } }
    }

    public class Student : Person
    {
        new ValueTuple<int, int> _value;

        public new ValueTuple<ValueTuple<int, string, string>, ValueTuple<int, int>> Value => ValueTuple.Create(base._value, _value);

        public Student() { }
        public Student(int id, string name, string address, int grade, int room) : base(id, name, address) { Grade = grade; Room = room; }
        public Student(ValueTuple<ValueTuple<int, string, string>, ValueTuple<int, int>> value) { _value = value.Item2; }

        public int Grade { get { return _value.Item1; } set { _value.Item1 = value; } }
        public int Room { get { return _value.Item2; } set { _value.Item2 = value; } }
    }
}
