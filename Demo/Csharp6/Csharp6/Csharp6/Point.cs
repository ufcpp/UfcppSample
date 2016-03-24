namespace Csharp6.Csharp6
{
    /// <summary>
    /// immutable なクラス/構造体を作るのに大変ありがたい構文が追加された。
    /// getter-only auto-property
    /// property initializer
    /// </summary>
    public class Point
    {
        public int X { get; } = 0;
        public int Y { get; } = 0;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int InnerProduct(Point p) => X * p.X + Y * p.Y;
        public static Point operator -(Point p) => new Point(-p.X, -p.Y);

        public override string ToString() => $"({X}, {Y})";
    }

    namespace PropertyInitializer
    {
        class Point
        {
            // プロパティに初期化子を付けれるようになった
            public int X { get; set; } = 10;
            public int Y { get; set; } = 20;
        }
    }

    namespace GetOnly
    {
        class Point
        {
            // ↓ set; を消すだけ
            public int X { get; } = 10;
            public int Y { get; } = 20;

        }
    }

    namespace ExpressionBodiedFunction
    {
        public class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Point(int x = 0, int y = 0) { X = x; Y = y; }

            public int InnerProduct(Point p) => X * p.X + Y * p.Y;
            public static Point operator -(Point p) => new Point(-p.X, -p.Y);
        }

    }
}
