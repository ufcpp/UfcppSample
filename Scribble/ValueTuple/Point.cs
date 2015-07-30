using System.Collections.Generic;

namespace ValueTuples
{
    public class Point : IRecord, IDeepCloneable<Point>
    {
        ValueTuple<int, int> _value;

        public ValueTuple<int, int> Value => _value;

        ITuple IRecord.Value { get { return _value; } set { _value = (ValueTuple<int, int>)value; } }

        private static readonly string[] _keys = new[] { "x", "y" };

        string IRecord.GetKey(int index) => _keys[index];

        int IRecord.GetIndex(string key)
        {
            switch (key)
            {
                default: return -1;
                case "x": return 0;
                case "y": return 1;
            }
        }

        int? IRecord.Discriminator => null;

        Point IDeepCloneable<Point>.Clone() => new Point(_value.DeepClone());

        public Point() { }
        public Point(int x, int y) { X = x; Y = y; }
        public Point(ValueTuple<int, int> value) { _value = value; }

        public int X { get { return _value.Item1; } set { _value.Item1 = value; } }
        public int Y { get { return _value.Item2; } set { _value.Item2 = value; } }

        public override string ToString() => $"({X}, {Y})";
    }
}
