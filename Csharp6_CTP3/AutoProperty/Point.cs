namespace AutoProperty.Base
{
    namespace Csharp2
    {
        class Point
        {
            private int _x;

            public int X
            {
                get { return _x; }
                set { _x = value; }
            }

            private int _y;

            public int Y
            {
                get { return _y; }
                set { _y = value; }
            }
        }
    }

    namespace Csharp5
    {
        class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
