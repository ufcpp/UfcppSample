namespace OverloadResolution.Inference
{
    using System;

    // 引数が完全に一致しているデリゲート型を2個用意
    delegate int A(int x);
    delegate int B(int x);

    class Program
    {
        static void Main()
        {
            // 2個以上候補があるときに default は使えない
#if Uncompilable
            M(default);
#endif

            // 型推論とはちょっと違うものの、null (型がない。どの型にでも代入可)でも同様
#if Uncompilable
            M(null);
#endif

            // 型指定ありの default なら大丈夫
            M(default(A));

            // A なのか B なのか区別がつかない
#if Uncompilable
            M(x => x);
#endif

            // キャストがあれば大丈夫
            // new でも可
            M((A)(x => x));
            M(new A(x => x));
        }

        static void M(A x) => Console.WriteLine("A");
        static void M(B x) => Console.WriteLine("B");
    }
}
