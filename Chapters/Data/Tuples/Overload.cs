using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuples.Overload
{
    class Program
    {
        static void Main()
        {

        }

        // 型違いのタプルでのオーバーロードは可能
        void F((int x, int y) t) { }
        void F((int x, string y) t) { }

#if false
        // 型が一緒で名前だけ違うタプルでのオーバーロードはダメ。コンパイル エラー
        void G((int x, int y) t) { }
        void G((int a, int b) t) { }
#endif
    }
}
