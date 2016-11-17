using SampleApp.Lib;
using System;
using System.Text.Utf8;

namespace SampleApp.Samples
{
    public class Entry : INamedMemberAccessor, IMemberAccessor, IRecordAccessor
    {
        private (int Code, string Name, long Id, byte Hash) _value;
        public ref (int Code, string Name, long Id, byte Hash) Value => ref _value;

        public int Code { get => _value.Code; set => _value.Code = value; }
        public string Name { get => _value.Name; set => _value.Name = value; }
        public long Id { get => _value.Id; set => _value.Id = value; }
        public byte Hash { get => _value.Hash; set => _value.Hash = value; }

        public override string ToString() => _value.ToString();

        NameIndexTable _table;
        int INamedMemberAccessor.GetIndex(Utf8String name)
        {
            if (_table.IsNull) _table = new NameIndexTable(typeof(Entry));
            return _table[name];
        }

        TypedPointer IMemberAccessor.GetPointer(int index) => ValueTupleAccessor.GetPointer(ref _value, index);

        Type IRecordAccessor.GetType(int index)
        {
            switch (index)
            {
                case 0: return typeof(int);
                case 1: return typeof(string);
                case 2: return typeof(long);
                case 3: return typeof(byte);
            }
            throw new IndexOutOfRangeException();
        }

        object IRecordAccessor.Get(int index)
        {
            switch (index)
            {
                case 0: return _value.Code;
                case 1: return _value.Name;
                case 2: return _value.Id;
                case 3: return _value.Hash;
            }
            throw new IndexOutOfRangeException();
        }

        void IRecordAccessor.Set(int index, object value)
        {
            switch (index)
            {
                case 0: _value.Code = (int)value; break;
                case 1: _value.Name = (string)value; break;
                case 2: _value.Id = (long)value; break;
                case 3: _value.Hash = (byte)value; break;
                default: throw new IndexOutOfRangeException();
            }
        }
    }
}
