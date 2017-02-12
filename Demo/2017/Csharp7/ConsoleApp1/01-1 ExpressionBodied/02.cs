using System;
using static System.Math;

namespace ConsoleApp1._01_ExpressionBodied2
{
    class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        // タプル・分解と組み合わせればコンストラクターも1行に (=> を使える)
        public Point(double x, double y) => (X, Y) = (x, y);

        ~Point() => Console.WriteLine("destructor");

        // set, get 個別に => を使えるように
        public double Radius
        {
            get => Sqrt(X * X + Y * Y);
            set => (X, Y) = (value * Cos(Theta), value * Sin(Theta));
        }

        public double Theta
        {
            get => Atan2(Y, X);
            set => (X, Y) = (Radius * Cos(value), Radius * Sin(value));
        }
    }
}
