using System;
using System.Collections.Generic;
using ValueTuples.Reflection;

namespace ValueTuples.Sample
{
    public partial class Point
    {
        public override string ToString() => $"({X}, {Y})";
    }

    public partial class Point
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Point() { }
        public Point(int x, int y) { X = x; Y = y; }

        static Point()
        {
            TypeRepository.Register(typeof(Point), new PointInfo());
        }
    }

    internal struct PointAccessor : IRecordAccessor
    {
        Point _value;

        public PointAccessor(Point value) { _value = value; }

        public int Count => 2;

        public object Get(string key)
        {
            switch (key)
            {
                case "X": return _value.X;
                case "Y": return _value.Y;
                default: return null;
            }
        }

        public object Get(int index)
        {
            switch (index)
            {
                case 0: return _value.X;
                case 1: return _value.Y;
                default: return null;
            }
        }

        public void Set(string key, object value)
        {
            switch (key)
            {
                case "X": _value.X = (int)value; break;
                case "Y": _value.Y = (int)value; break;
            }
        }

        public void Set(int index, object value)
        {
            switch (index)
            {
                case 0: _value.X = (int)value; break;
                case 1: _value.Y = (int)value; break;
            }
        }
    }


    internal class PointInfo : RecordTypeInfo
    {
        public override Type Type => typeof(Point);

        private static readonly RecordFieldInfo[] _fields =
        {
            new RecordFieldInfo(TypeRepository.Int32, "X", 0),
            new RecordFieldInfo(TypeRepository.Int32, "Y", 1),
        };

        public override IEnumerable<RecordFieldInfo> Fields => _fields;

        public override object GetInstance() => new Point();
        public override Array GetArray(int length) => new Point[length];

        public override IRecordAccessor GetAccessor(object instance) => new PointAccessor((Point)instance);
    }
}
