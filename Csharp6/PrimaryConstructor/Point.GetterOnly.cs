namespace AutoProperty.GetterOnly
{
    namespace Csharp3
    {
        class Point
        {
            private readonly int _x; // ←ここと
            public int X { get { return _x; } } // ←ここと

            private readonly int _y;
            public int Y { get { return _y; } }

            public Point(int x, int y) // ←ここと
            {
                _x = x; // ←ここ、何か所に x と書けばいいのか
                _y = y;
            }

            public Point() : this(0, 0) { }
        }
    }

    namespace Csharp6
    {
        class Point(int x, int y) // ←ここと
        {
            public int X { get; } = x; // ←ここだけ
            public int Y { get; } = y;

            public Point() : this(0, 0) { }

            // ちなみに、以下のようなコードを書くとエラーになる。
            // public Point() { }
            // 理由は、プライマリ コンストラクターを呼んでいないから。
            // プライマリ コンストラクターは必ず呼ばれないと行けない。
        }
    }
}
