using System;
using static System.Math;

namespace ConsoleApp1._01_ExpressionBodied1
{
    // コンストラクター、デストラクター、プロパティの set/get、イベントの add/remove でも
    // => を使えるようになった
    // タプルとの組み合わせが結構有効なのでここで紹介
    class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        ~Point()
        {
            Console.WriteLine("destructor");
        }

        public double Radius
        {
            get { return Sqrt(X * X + Y * Y); }
            set
            {
                X = value * Cos(Theta);
                Y = value * Sin(Theta);
            }
        }

        public double Theta
        {
            get { return Atan2(Y, X); }
            set
            {
                X = Radius * Cos(value);
                Y = Radius * Sin(value);
            }
        }
    }
}
