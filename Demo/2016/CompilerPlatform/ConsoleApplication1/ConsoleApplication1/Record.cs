using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Record
{
    namespace 意味のある情報
    {
        struct Point
        {
            public int X { get; }
            public int Y { get; }
        }
    }

    namespace 来るべきレコード型
    {
#if false
        struct Point(int X, int Y);
#endif
    }

    namespace こう書かなきゃいけない
    {
        struct Point
        {
            public int X { get; }
            public int Y { get; }
            public Point(int x, int y) { X = x; Y = y; }
            public void Deconstruct(out int x, out int y) { x = X; y = Y; }
            // Equals, GetHashCode, ==, !=, With...
        }
    }
}
