using System;
using System.Collections.Generic;
using System.Text;

namespace MetricSpace.Devirtualized2
{
    struct Array1<T>
    {
        public T Item1;
    }

    struct Array2<T>
    {
        public T Item1;
        public T Item2;
    }

    struct Array3<T>
    {
        public T Item1;
        public T Item2;
        public T Item3;
    }

#if Uncompilable
    struct Array2<T>
    {
        public T Item1;
        public T Item2;

        // これをジェネリックに使いたければトリックが必要
        public static int Length => 2;

        // ただでさえ、safe にインデックス アクセスを実現する方法はないんだけど…
        // そもそも、ref Item1 したものを、ref 戻り値では返せない仕様
        public ref T this[int index] => ref Unsafe.Add<T>(ref Item1, index);
    }
#endif
}
