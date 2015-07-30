using System;
using System.Collections.Generic;
using ValueTuples.Reflection;

namespace ValueTuples.Sample
{
    public class Fighter : Unit
    {
        public double Strength { get; set; }

        public Fighter() { }
        public Fighter(int id, string name, double strength) : base(id, name) { Strength = strength; }

        static Fighter()
        {
            TypeRepository.Register(typeof(Fighter), new FighterInfo());
        }
    }

    internal struct FighterAccessor : IRecordAccessor
    {
        Fighter _value;

        public FighterAccessor(Fighter value) { _value = value; }

        public int Count => 2;

        public object Get(string key)
        {
            switch (key)
            {
                case "Id": return _value.Id;
                case "Name": return _value.Name;
                case "Strength": return _value.Strength;
                default: return null;
            }
        }

        public object Get(int index)
        {
            switch (index)
            {
                case 0: return _value.Id;
                case 1: return _value.Name;
                case 2: return _value.Strength;
                default: return null;
            }
        }

        public void Set(string key, object value)
        {
            switch (key)
            {
                case "Id": _value.Id = (int)value; break;
                case "Name": _value.Name = (string)value; break;
                case "Strength": _value.Strength = (double)value; break;
            }
        }

        public void Set(int index, object value)
        {
            switch (index)
            {
                case 0: _value.Id = (int)value; break;
                case 1: _value.Name = (string)value; break;
                case 2: _value.Strength = (double)value; break;
            }
        }
    }

    internal class FighterInfo : RecordTypeInfo
    {
        public override Type Type => typeof(Fighter);

        private static readonly RecordFieldInfo[] _fields =
        {
            new RecordFieldInfo(TypeRepository.Int32, "Id", 0),
            new RecordFieldInfo(TypeRepository.String, "Name", 1),
            new RecordFieldInfo(TypeRepository.Double, "Strength", 2),
        };

        public override IEnumerable<RecordFieldInfo> Fields => _fields;

        public override object GetInstance() => new Fighter();
        public override Array GetArray(int length) => new Fighter[length];

        public override IRecordAccessor GetAccessor(object instance) => new FighterAccessor((Fighter)instance);
    }
}
