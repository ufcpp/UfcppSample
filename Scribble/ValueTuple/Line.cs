using System.Collections.Generic;

namespace ValueTuples
{
    public class Line : IRecord, IDeepCloneable<Line>
    {
        ValueTuple<Point, Point> _value;

        public ValueTuple<Point, Point> Value => _value;

        ITuple IRecord.Value { get { return _value; } set { _value = (ValueTuple<Point, Point>)value; } }

        private static readonly string[] _keys = new[] { "a", "b" };

        string IRecord.GetKey(int index) => _keys[index];

        int IRecord.GetIndex(string key)
        {
            switch (key)
            {
                default: return -1;
                case "a": return 0;
                case "b": return 1;
            }
        }

        int? IRecord.Discriminator => null;

        Line IDeepCloneable<Line>.Clone() => new Line(_value.DeepClone());

        public Line() { }
        public Line(Point a, Point b) { A = a; B = b; }
        public Line(ValueTuple<Point, Point> value) { _value = value; }

        public Point A { get { return _value.Item1; } set { _value.Item1 = value; } }
        public Point B { get { return _value.Item2; } set { _value.Item2 = value; } }

        public override string ToString() => $"{A} - {B}";
    }
}
