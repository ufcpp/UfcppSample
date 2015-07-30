using System;
using System.Collections.Generic;
using ValueTuples.Reflection;

namespace ValueTuples.Sample
{
    public enum UnitType
    {
        Fighter,
        Mage,
        Thief,
    }

    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Unit() { }
        public Unit(int id, string name) { Id = id; Name = name; }

        static Unit()
        {
            TypeRepository.Register(typeof(Unit), new UnitInfo());
        }
    }

    internal struct UnitAccessor : IRecordAccessor
    {
        Unit _value;

        public UnitAccessor(Unit value) { _value = value; }

        public object Get(string key)
        {
            switch (key)
            {
                case "Id": return _value.Id;
                case "Name": return _value.Name;
                default: return null;
            }
        }

        public object Get(int index)
        {
            switch (index)
            {
                case 0: return _value.Id;
                case 1: return _value.Name;
                default: return null;
            }
        }

        public void Set(string key, object value)
        {
            switch (key)
            {
                case "Id": _value.Id = (int)value; break;
                case "Name": _value.Name = (string)value; break;
            }
        }

        public void Set(int index, object value)
        {
            switch (index)
            {
                case 0: _value.Id = (int)value; break;
                case 1: _value.Name = (string)value; break;
            }
        }
    }

    internal class UnitInfo : RecordTypeInfo
    {
        public override Type Type => typeof(Unit);

        public override bool IsBase => true;

        private static readonly RecordFieldInfo[] _fields =
        {
            new RecordFieldInfo(TypeRepository.Int32, "Id", 0),
            new RecordFieldInfo(TypeRepository.String, "Name", 1),
        };

        public override IEnumerable<RecordFieldInfo> Fields => _fields;

        public override object GetInstance() => new Unit();
        public override Array GetArray(int length) => new Unit[length];

        public override IRecordAccessor GetAccessor(object instance) => new UnitAccessor((Unit)instance);

        public override RecordTypeInfo GetType(int discriminator)
        {
            switch ((UnitType)discriminator)
            {
                case UnitType.Fighter: return TypeRepository.Get(typeof(Fighter));
                case UnitType.Mage: return TypeRepository.Get(typeof(Mage));
                case UnitType.Thief: return TypeRepository.Get(typeof(Thief));
                default: return null; // throw
            }
        }
    }
}
