using System;

namespace PrimaryConstructor
{
    namespace Csharp5
    {
        /// <summary>
        /// 第1象限 = x, y ともに正の領域
        /// </summary>
        class FirstQuadrant
        {
            public int X { get; private set; }
            public int Y { get; private set; }

            public FirstQuadrant(int x, int y)
            {
                if (x < 0) throw new ArgumentException("x must be positive.");
                if (y < 0) throw new ArgumentException("y must be positive.");

                X = x;
                Y = y;
            }
        }
    }

    namespace Csharp6
    {
        class FirstQuadrant(int x, int y)
        {
            {
                if (x < 0) throw new ArgumentException(nameof(x) + "must be positive.");
                if (y < 0) throw new ArgumentException(nameof(y) + "must be positive.");
            }

            public int X { get; } = x;
            public int Y { get; } = y;
        }
    }
}
