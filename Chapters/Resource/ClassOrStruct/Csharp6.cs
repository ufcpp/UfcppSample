using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Csharp5_Error
{
#if false
    struct Sample1
    {
        int _x;
        int _y;
        int _z;

        public Sample1(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }

    struct Sample2
    {
        int _x;
        int _y;

        public Sample2(int x, int y)
        {
            M(); // エラー: _x, _y の初期化より前に呼んじゃダメ。
            _x = x;
            _y = y;
            M(); // この順ならOK。
        }

        void M() { }
    }
#endif

    public struct Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(int x, int y)
        {
            // C# 5.0 まではエラーになる
            X = y;
            Y = y;
        }
    }
}

namespace Csharp5
{
    public struct Point
    {
        private int _x;
        public int X { get { return _x; } }

        private int _y;
        public int Y { get { return _y; } }

        public Point(int x, int y)
        {
            _x = y;
            _y = y;
        }
    }
}

namespace Csharp6
{
    public struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = y;
            Y = y;
        }
    }
}
