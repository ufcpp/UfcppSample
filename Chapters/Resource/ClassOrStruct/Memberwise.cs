namespace Memberwise
{
    using System;

    public class Program
    {
        public struct Point
        {
            public int X { get; }
            public int Y { get; }

            public Point(int x, int y) { X = y; Y = y; }
            public override string ToString() => $"({X}, {Y})";
        }

        public static void Main(string[] args)
        {
            var x = new Point(1, 2);
            var y = x;

            Console.WriteLine(y); // x のメンバー毎コピー = (1, 2)

            // メンバー毎比較(全メンバー一致なら一致)
            Console.WriteLine(x.Equals(new Point(1, 2))); // true
            Console.WriteLine(x.Equals(new Point(1, 3))); // false
        }
    }
}
