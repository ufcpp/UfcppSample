using System;
using System.Text.Utf8;
using SampleApp.Lib;

namespace SampleApp.Samples
{
    public class Point : INamedMemberAccessor, IMemberAccessor, IRecordAccessor
    {
        private (int X, int Y, int Z) _value;
        public ref (int X, int Y, int Z) Value => ref _value;

        public int X { get => _value.X; set => _value.X = value; }
        public int Y { get => _value.Y; set => _value.Y = value; }
        public int Z { get => _value.Z; set => _value.Z = value; }

        public override string ToString() => _value.ToString();

        NameIndexTable _table;
        int INamedMemberAccessor.GetIndex(Utf8String name)
        {
            if (_table.IsNull) _table = new NameIndexTable(typeof(Point));
            return _table[name];
        }

        TypedPointer IMemberAccessor.GetPointer(int index) => ValueTupleAccessor.GetPointer(ref _value, index);

        Type IRecordAccessor.GetType(int index)
        {
            switch (index)
            {
                case 0: return typeof(int);
                case 1: return typeof(int);
                case 2: return typeof(int);
            }
            throw new IndexOutOfRangeException();
        }

        object IRecordAccessor.Get(int index)
        {
            switch (index)
            {
                case 0: return _value.X;
                case 1: return _value.Y;
                case 2: return _value.Z;
            }
            throw new IndexOutOfRangeException();
        }

        void IRecordAccessor.Set(int index, object value)
        {
            switch (index)
            {
                case 0: _value.X = (int)value; break;
                case 1: _value.Y = (int)value; break;
                case 2: _value.Z = (int)value; break;
                default: throw new IndexOutOfRangeException();
            }
        }
    }
}
