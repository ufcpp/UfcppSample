namespace ConsoleApp1.RefExtensions.Before
{
    using System;

    struct Point
    {
        public int X;
        public int Y;
        public int Z;

        public void IncX() => this.X++;

        public override string ToString() => $"{X}, {Y}, {Z}";
    }

    static class PointExtensions
    {
        public static void IncY(this Point @this) => @this.Y++;
        public static void IncZ(ref Point @this) => @this.Z++;
    }

    class Program
    {
        static void Main()
        {
            var p = new Point();
            p.IncX(); // 普通のメソッド: X は +1 される
            p.IncY(); // 拡張メソッド: p のコピーが発生。コピーの方のYが書き換わる。p.Y は変化なし
            PointExtensions.IncZ(ref p); // これなら、p.Z が変化する

            Console.WriteLine(p); // 1, 0, 1
        }
    }
}
