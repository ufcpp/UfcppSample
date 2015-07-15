namespace ParameterlessConstructor
{
    using System;

    struct Point
    {
        public int X { get; }
        public int Y { get; }
        public Point(int x, int y) { X = x; Y = y; }
        public override string ToString() => $"({X}, {Y})";
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var p1 = new Point();
            var p2 = new Point(10, 20);
            var p3 = default(Point); // C# 2.0 以降。p1と同じ意味

            Console.WriteLine(p1);
            Console.WriteLine(p2);
            Console.WriteLine(p3);
        }
    }
}
