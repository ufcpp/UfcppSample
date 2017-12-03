#pragma warning disable 649

namespace StructLayoutSample.IllegalAddress
{
    using System;

    struct Sample
    {
        public int I;
        public string S;
    }

    class Program
    {
        static unsafe void Main()
        {
#if false
            var a = default(Sample);
            var p = &a;    // コンパイル エラー: 参照型を含んだ構造体はアドレス取れない
            var pi = &a.I;
            var ps = &a.S; // コンパイル エラー: 参照型メンバーのアドレスは取れない

            Console.WriteLine((long)pi - (long)p);
            Console.WriteLine((long)ps - (long)p);
#endif
        }
    }
}
