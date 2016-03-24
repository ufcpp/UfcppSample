using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp6.Misc
{
    class SmallChanges
    {
    }

    struct Point
    {
        public int X { get; private set; }

        public Point(int x)
        {
            // C# 5.0まではエラーに。
            X = x;
        }
    }
}
