using System;

namespace ConsoleApplication1
{
    interface Shape
    {
    }

    class Rectangle : Shape
    {
        public double 幅 = 0;
        public double 高さ = 0;
    }

    class Circle : Shape
    {
        public double 半径 = 0;
    }

    static class ShapeMethods
    {
        //public static double GetArea(this Shape s)
        //{
        //    if (s is Rectangle) return GetArea((Rectangle)s);
        //    if (s is Circle) return GetArea((Circle)s);
        //    throw new ArgumentException();
        //}
        // ↑before
        // ↓after
        public static double GetArea(this Shape s)
        {
            return GetArea((dynamic)s);
        }
        static double GetArea(dynamic s) { return GetArea(s); }

        static double GetArea(Rectangle x) { return x.幅 * x.高さ; }
        static double GetArea(Circle x) { return Math.PI * x.半径 * x.半径; }

        //public static bool Contains(this Shape s, Shape t)
        //{
        //    if (s is Rectangle && t is Rectangle) return Contains((Rectangle)s, (Rectangle)t);
        //    if (s is Rectangle && t is Circle) return Contains((Rectangle)s, (Circle)t);
        //    if (s is Circle && t is Rectangle) return Contains((Circle)s, (Rectangle)t);
        //    if (s is Circle && t is Circle) return Contains((Circle)s, (Circle)t);
        //    throw new ArgumentException();
        //}
        // ↑before
        // ↓after
        public static bool Contains(this Shape s, Shape t)
        {
            return Contains((dynamic)s, (dynamic)t);
        }
        static bool Contains(dynamic s, dynamic t) { return Contains(s, t); }

        static bool Contains(Rectangle s, Rectangle t)
        {
            return s.幅 > t.幅 && s.高さ > t.高さ;
        }
        static bool Contains(Rectangle s, Circle t)
        {
            return s.幅 * s.幅 + s.高さ * s.高さ > t.半径 * t.半径 * 4;
        }
        static bool Contains(Circle s, Rectangle t)
        {
            return s.半径 * s.半径 * 4 > t.幅 * t.幅 + t.高さ * t.高さ;
        }
        static bool Contains(Circle s, Circle t)
        {
            return s.半径 > t.半径;
        }
    }

    public class MultipleDispatchSample
    {
        public static void Test()
        {
            Shape s = new Rectangle { 幅 = 2, 高さ = 3 };
            Shape t = new Circle { 半径 = 2 };
            Console.WriteLine(s.Contains(t));
            Console.WriteLine(t.Contains(s));
        }
    }
}
