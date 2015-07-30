using System;
using System.Collections.Generic;
using ValueTuples.Reflection;

namespace ValueTuples.Sample
{
    public partial class Pair<T1, T2>
    {
        public override string ToString() => $"({Item1}, {Item2})";
    }

    public partial class Pair<T1, T2>
    {
        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }

        public Pair() { }
        public Pair(T1 item1, T2 item2) { Item1 = item1; Item2 = item2; }

        static Pair()
        {
            TypeRepository.Register(typeof(Pair<T1, T2>), new PairInfo<T1, T2>());
        }
    }

    internal struct PairAccessor<T1, T2> : IRecordAccessor
    {
        Pair<T1, T2> _value;

        public PairAccessor(Pair<T1, T2> value) { _value = value; }

        public int Count => 2;

        public object Get(string key)
        {
            switch (key)
            {
                case "Item1": return _value.Item1;
                case "Item2": return _value.Item2;
                default: return null;
            }
        }

        public object Get(int index)
        {
            switch (index)
            {
                case 0: return _value.Item1;
                case 1: return _value.Item2;
                default: return null;
            }
        }

        public void Set(string key, object value)
        {
            switch (key)
            {
                case "Item1": _value.Item1 = (T1)value; break;
                case "Item2": _value.Item2 = (T2)value; break;
            }
        }

        public void Set(int index, object value)
        {
            switch (index)
            {
                case 0: _value.Item1 = (T1)value; break;
                case 1: _value.Item2 = (T2)value; break;
            }
        }
    }


    internal class PairInfo<T1, T2> : RecordTypeInfo
    {
        public override Type Type => typeof(Pair<T1, T2>);

        private static readonly RecordFieldInfo[] _fields =
        {
            new RecordFieldInfo(TypeRepository.Get(typeof(T1)), "Item1", 0),
            new RecordFieldInfo(TypeRepository.Get(typeof(T2)), "Item2", 1),
        };

        public override IEnumerable<RecordFieldInfo> Fields => _fields;

        public override object GetInstance() => new Pair<T1, T2>();
        public override Array GetArray(int length) => new Pair<T1, T2>[length];

        public override IRecordAccessor GetAccessor(object instance) => new PairAccessor<T1, T2>((Pair<T1, T2>)instance);
    }
}
