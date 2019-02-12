namespace Patterns.PositionalPattern
{
    class Program
    {
        static void Main()
        {
            // 位置指定で構築できるんなら、位置指定でマッチングできるべき
            var p1 = new Point(1, 2);
            var r1 = p1 is Point (1, 2);

            // 名前指定で構築できるんなら、名前指定でマッチングできるべき
            var p2 = new Point(x: 1, y: 2);
            var r2 = p2 is Point (x: 1, y: 2);

#if false // 実装待ち。C# 8.0 候補には入ってる
            // 型推論が効く場合に new の後ろの型名は省略可能(になる予定)なら
            // 型が既知なら型名を省略してマッチングできるべき
            Point p3 = new (1, 2);
            var r3 = p2 is (1, 2);
#endif

            // 階層的に new できるんなら、階層的にマッチングできるべき
            var line = new Line(new Point(1, 2), new Point(3, 4));
            var r4 = line is ((1, 2), (3, 4));
        }

        static int M(Point p)
            => p switch
        {
            (1, 2) => 0,
            (var x, _) when x > 0 => x,
            _ => -1
        };

        static int M1(Point p)
        {
            p.Deconstruct(out var x, out var y);
            if (x is 1 && y is 2) return 0;
            if (x > 0) return x;
            return -1;
        }

        static int ExplicitType(object obj)
            => obj switch
        {
            int i => i,
            string s => s.Length,
            Point(var x, var y) => 0,
            _ => -1
        };

        static int MixPatterns(object obj)
            => obj switch
        {
            Point (var x, _) { Y: var y } p => x * y
        };

        static int NamedPattern(Point p)
            => p switch
        {
            (x: 1, y: 2) => 0,
            (x: var x, y: _) when x > 0 => x,
            _ => -1
        };
    }
}
