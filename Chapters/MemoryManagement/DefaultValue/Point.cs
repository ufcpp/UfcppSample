using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultValue
{
    struct Point
    {
        public int X;
        public int Y;

        // C# 5.0 以前ではコンパイル エラーになる
        public Point()
        {
            X = int.MinValue;
            Y = int.MinValue;
        }
    }

    struct Entry
    {
        public int Id = 0;       // C# 5.0 以前ではコンパイル エラーになる
        public string Name = ""; // ここも
    }
}
