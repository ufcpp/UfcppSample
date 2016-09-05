namespace OutVar.Point
{
    using System;

    struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public void GetCoordinate(out int x, out int y)
        {
            x = X;
            y = Y;
        }
    }

    class Program
    {
        static void Main()
        {
            // x, y のスコープはこのブロック内
            // この辺りで x, y という名前の変数は作れない

            var p = new Point { X = 1, Y = 2 };
            p.GetCoordinate(out var x, out var y);

            // 以下のような書き方をしたのと同じ
            // int x, y;
            // p.GetCoordinate(out x, out y);

            // この行から下で x, y を使える

            Console.WriteLine($"{x}, {y}");
        }
    }
}
