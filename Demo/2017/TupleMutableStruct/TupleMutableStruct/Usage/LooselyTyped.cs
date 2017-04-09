namespace TupleMutableStruct.Usage.LooselyTyped
{
    namespace ImmutableStruct
    {
        // X, Y を持つ書き換え不能な構造体
        struct Point
        {
            public readonly (int X, int Y) Value;
            public int X => Value.X;
            public int Y => Value.Y;
            public Point(int x, int y) => Value = (x, y);
            public Point((int X, int Y) value) => Value = value;
        }
    }

    namespace MutableClass
    {
        // X, Y を持つ通常のクラス
        class Point
        {
            public (int X, int Y) Value;
            public int X { get => Value.X; set => Value.X = value; }
            public int Y { get => Value.Y; set => Value.Y = value; }
        }
    }

    namespace ObserverbleClass
    {
        // X, Y を持つ変更通知付きのクラス
        class Point : BindableBase
        {
            public (int X, int Y) Value;
            public int X { get => Value.X; set => SetProperty(ref Value.X, value); }
            public int Y { get => Value.Y; set => SetProperty(ref Value.Y, value); }
        }
    }
}
