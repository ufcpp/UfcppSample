namespace Csharp6.Csharp5
{
    /// <summary>
    /// (X, Y) なクラス。X も Y も immutable に作りたい。
    /// その場合、結構煩雑な定型文になってしまう…
    /// </summary>
    class Point
    {
        private readonly int _x; // ←ここと
        private readonly int _y;

        public Point(int x, int y) // ←ここと
        {
            _x = x; // ←ここと
            _y = y;
        }

        public int X { get { return _x; } } // ←ここ、何回 x 書けばいいんだよ！
        public int Y { get { return _y; } }

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }
    }

    namespace PropertyInitializerWithoutAutoProperty
    {
        class Point
        {
            // ↓この初期値を設定するためだけに自動実装をやめることに
            private int _x = 10;

            public int X
            {
                get { return _x; }
                set { _x = value; }
            }

            private int _y = 20;

            public int Y
            {
                get { return _y; }
                set { _y = value; }
            }
        }
    }

    namespace PropertyInitializerWithConstructor
    {
        class Point
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Point()
            {
                // ↓この初期値を設定するためだけにコンストラクターが必要
                X = 10;
                Y = 20;
            }
        }
    }

    namespace GetOnlyWithoutAutoProperty
    {
        class Point
        {
            // ↓getのみの自動実装はできないので仕方なくフィールドを用意
            private readonly int _x = 10;
            public int X { get { return _x; } }

            private readonly int _y = 20;
            public int Y { get { return _y; } }
        }
    }

    namespace GetOnlyWithConstructor
    {
        class Point
        {
            // ↓setをprivateにすることで外からは書き替えれないように
            public int X { get; private set; }
            public int Y { get; private set; }

            public Point()
            {
                X = 10;
                Y = 20;
            }
        }
    }
}
