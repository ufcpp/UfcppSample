namespace Csharp6.Csharp6
{
    /// <summary>
    /// immutable なクラス/構造体を作るのに大変ありがたい構文が追加された。
    /// getter-only auto-property
    /// property initializer
    /// </summary>
    class Point
    {
        public int X { get; } = 0;
        public int Y { get; } = 0;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

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
}
