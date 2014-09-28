namespace AutoProperty.Initializer
{
    namespace Csharp5.Field
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

    namespace Csharp5.Constructor
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

    namespace Csharp6
    {
        class Point
        {
            public int X { get; set; } = 10;
            public int Y { get; set; } = 20;
        }
    }
}
