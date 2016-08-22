using System;
using System.Collections.Generic;

namespace Deconstruction.DeconstructMethod
{
    class Program
    {
        static void Main()
        {
            var pair = new KeyValuePair<string, int>("one", 1);
            var (k, v) = pair;
            // 以下のようなコードに展開される
            // string k;
            // int v;
            // pair.Deconstruct(out k, out v);

            var tuple = Tuple.Create("abc", 100);
            var (x, y) = tuple;
            // 以下のようなコードに展開される
            // string x;
            // int y;
            // tuple.Deconstruct(out x, out y);
        }
    }
}
