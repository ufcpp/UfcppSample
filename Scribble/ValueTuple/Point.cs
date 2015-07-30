using System.Collections.Generic;

namespace ValueTuples
{
    public class Point
    {
        ValueTuple<int, int> _value;

        public ValueTuple<int, int> Value => _value;

        public static readonly IEnumerable<string> Keys = new[] { "x", "y" };

        public static int GetIndex(string key)
        {
            switch (key)
            {
                default: return -1;
                case "x": return 0;
                case "y": return 1;
            }
        }

        public Point() { }
        public Point(int x, int y) { X = x; Y = y; }
        public Point(ValueTuple<int, int> value) { _value = value; }

        public int X { get { return _value.Item1; } set { _value.Item1 = value; } }
        public int Y { get { return _value.Item2; } set { _value.Item2 = value; } }
    }
}
