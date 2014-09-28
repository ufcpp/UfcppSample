namespace AutoProperty.GetterOnly
{
    namespace Csharp5.Field
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

    namespace Csharp5.Constructor
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

    namespace Csharp6
    {
        class Point
        {
            // ↓ set; を消すだけ
            public int X { get; } = 10;
            public int Y { get; } = 20;
        }
    }
}
