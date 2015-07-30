using System;
using System.Collections.Generic;
using ValueTuples.Reflection;

namespace ValueTuples.Sample
{
    public class Mage : Unit
    {
        public string Spell { get; set; }

        public Mage() { }
        public Mage(int id, string name, string spell) : base(id, name) { Spell = spell; }

        static Mage()
        {
            TypeRepository.Register(typeof(Mage), new MageInfo());
        }
    }

    internal struct MageAccessor : IRecordAccessor
    {
        Mage _value;

        public MageAccessor(Mage value) { _value = value; }

        public object Get(string key)
        {
            switch (key)
            {
                case "Id": return _value.Id;
                case "Name": return _value.Name;
                case "Spell": return _value.Spell;
                default: return null;
            }
        }

        public object Get(int index)
        {
            switch (index)
            {
                case 0: return _value.Id;
                case 1: return _value.Name;
                case 2: return _value.Spell;
                default: return null;
            }
        }

        public void Set(string key, object value)
        {
            switch (key)
            {
                case "Id": _value.Id = (int)value; break;
                case "Name": _value.Name = (string)value; break;
                case "Spell": _value.Spell = (string)value; break;
            }
        }

        public void Set(int index, object value)
        {
            switch (index)
            {
                case 0: _value.Id = (int)value; break;
                case 1: _value.Name = (string)value; break;
                case 2: _value.Spell = (string)value; break;
            }
        }
    }

    internal class MageInfo : RecordTypeInfo
    {
        public override Type Type => typeof(Mage);

        public override int? Discriminator => (int)UnitType.Mage;

        private static readonly RecordFieldInfo[] _fields =
        {
            new RecordFieldInfo(TypeRepository.Int32, "Id", 0),
            new RecordFieldInfo(TypeRepository.String, "Name", 1),
            new RecordFieldInfo(TypeRepository.String, "Spell", 2),
        };

        public override IEnumerable<RecordFieldInfo> Fields => _fields;

        public override object GetInstance() => new Mage();
        public override Array GetArray(int length) => new Mage[length];

        public override IRecordAccessor GetAccessor(object instance) => new MageAccessor((Mage)instance);
    }
}
