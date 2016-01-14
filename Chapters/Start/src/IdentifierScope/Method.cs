using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentifierScope
{
    public class Sample
    {
        public static int X(int X)
        {
            if (X <= 1) return 1;
            else return Sample.X(X - 1);
        }

        public static void M(int x)
        {
            //int x = 10; // コンパイル エラー
            Console.WriteLine(x);
        }
    }
}
