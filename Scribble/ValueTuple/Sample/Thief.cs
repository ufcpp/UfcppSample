using System;
using System.Collections.Generic;
using ValueTuples.Reflection;

namespace ValueTuples.Sample
{
    public class Thief : Unit
    {
        public int Agility { get; set; }
        public int Skill { get; set; }

        public Thief() { }
        public Thief(int id, string name, int agility, int skill) : base(id, name) { Agility = agility; Skill = skill; }

        static Thief()
        {
            TypeRepository.Register(typeof(Thief), new ThiefInfo());
        }
    }

    internal struct ThiefAccessor : IRecordAccessor
    {
        Thief _value;

        public ThiefAccessor(Thief value) { _value = value; }

        public int Count => 2;

        public object Get(string key)
        {
            switch (key)
            {
                case "Id": return _value.Id;
                case "Name": return _value.Name;
                case "Agility": return _value.Agility;
                case "Skill": return _value.Skill;
                default: return null;
            }
        }

        public object Get(int index)
        {
            switch (index)
            {
                case 0: return _value.Id;
                case 1: return _value.Name;
                case 2: return _value.Agility;
                case 3: return _value.Skill;
                default: return null;
            }
        }

        public void Set(string key, object value)
        {
            switch (key)
            {
                case "Id": _value.Id = (int)value; break;
                case "Name": _value.Name = (string)value; break;
                case "Agility": _value.Agility = (int)value; break;
                case "Skill": _value.Skill = (int)value; break;
            }
        }

        public void Set(int index, object value)
        {
            switch (index)
            {
                case 0: _value.Id = (int)value; break;
                case 1: _value.Name = (string)value; break;
                case 2: _value.Agility = (int)value; break;
                case 3: _value.Skill = (int)value; break;
            }
        }
    }

    internal class ThiefInfo : RecordTypeInfo
    {
        public override Type Type => typeof(Thief);

        private static readonly RecordFieldInfo[] _fields =
        {
            new RecordFieldInfo(TypeRepository.Int32, "Id", 0),
            new RecordFieldInfo(TypeRepository.String, "Name", 1),
            new RecordFieldInfo(TypeRepository.Int32, "Agility", 2),
            new RecordFieldInfo(TypeRepository.Int32, "Skill", 2),
        };

        public override IEnumerable<RecordFieldInfo> Fields => _fields;

        public override object GetInstance() => new Thief();
        public override Array GetArray(int length) => new Thief[length];

        public override IRecordAccessor GetAccessor(object instance) => new ThiefAccessor((Thief)instance);
    }
}
