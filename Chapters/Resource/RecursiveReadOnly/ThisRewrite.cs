namespace RecursiveReadOnly.ThisRewrite
{
    using System;

    struct Point
    {
        // フィールドに readonly を付けているものの…
        public readonly int X;
        public readonly int Y;

        public Point(int x, int y) => (X, Y) = (x, y);

        // this の書き換えができてしまうので、実は X, Y の書き換えが可能
        public void Set(int x, int y)
        {
            // X = x; Y = y; とは書けない
            // でも、this 自体は書き換えられる
            this = new Point(x, y);
        }
    }

    class Program
    {
        static void Main()
        {
            var p = new Point(1, 2);

            // p.X = 0; とは書けない。これはちゃんとコンパイル エラーになる

            // でも、このメソッドは呼べるし、X, Y が書き換わる
            p.Set(3, 4);

            Console.WriteLine(p.X); // 3
            Console.WriteLine(p.Y); // 4
        }
    }
}
