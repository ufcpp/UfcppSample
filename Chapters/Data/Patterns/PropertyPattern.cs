namespace Patterns.PropertyPattern
{
    class Program
    {
        public static void Main()
        {
            // 初期化子でプロパティ指定できるんなら、プロパティ指定でマッチングできるべき
            var p1 = new Point { X = 1, Y = 2 };
            var r1 = p1 is { X: 1, Y: 2 };

            // 混在で構築できるんなら、混在でマッチングできるべき
            var p2 = new Point(x: 1) { Y = 2 };
            var r2 = p2 is (1, _) { Y: 2 };
        }

        static int M(Point p)
            => p switch
        {
            { X: 1, Y: 2 } => 0,
            { X: var x, Y: _ } when x > 0 => x,
            _ => -1
        };

        static int M1(Point p)
        {
            var x = p.X;
            var y = p.Y;
            if (x is 1 && y is 2) return 0;
            if (x > 0) return x;
            return -1;
        }

        static int M(object obj)
            => obj switch
        {
            int i => i,
            string s => s.Length,
            Point { X: 0, Y: 0 } => 0,
            Point (_, _) => 1,
            _ => -1
        };
    }
}
