using System;
using System.Collections.Generic;
using System.Text;

namespace Patterns.ConstantPattern
{
    class Program
    {
        static void Main()
        {
        }

        static int M(object x)
        {
            switch (x)
            {
                // 定数パターン
                case 0: return 0;
                // 型パターン
                case string s: return s.Length;
                default: return -1;
            }
        }

        static int M(object x, int comparand)
        {
            switch (x)
            {
                // case comparand: とは書けない。
                // 型パターン + when 句を使う。
                case int i when i == comparand: return 0;
                default: return -1;
            }
        }
    }
}
