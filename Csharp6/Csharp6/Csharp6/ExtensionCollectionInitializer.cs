namespace Csharp6.Csharp6.ExtensionCollectionInitializer
{
    using System.Collections.Generic;

    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    static class PointExtensions
    {
        public static void Add(this List<Point> list, int x, int y)
            => list.Add(new Point { X = x, Y = y });
    }

    class Program
    {
        static void Main()
        {
            var points = new List<Point>
            {
                // PointExtensions.Add が呼ばれる
                { 1, 2 },
                { 4, 6 },
                { 0, 3 },
            };
        }
    }
}
