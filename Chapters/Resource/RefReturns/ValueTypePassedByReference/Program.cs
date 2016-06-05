namespace RefReturns.ValueTypePassedByReference
{
    using System;

    struct Point
    {
        public int X;
        public int Y;

        public override string ToString() => $"({X}, {Y})";
    }

    class Program
    {
        public static void Main()
        {
            var p = new Point { X = 1, Y = 2 };

            // p のコピーが作られる
            var q = p;

            // コピー側の書き換えなので、p には影響なし
            q.X = 3;
            Console.WriteLine(p); // 1, 2
            Console.WriteLine(q); // 3, 2

            // 同じく、p を書き換えても q に影響なし
            p.Y = 4;
            Console.WriteLine(p); // 1, 4
            Console.WriteLine(q); // 3, 2
        }
    }
}
