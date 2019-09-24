namespace OverloadResolution.Lambda
{
    using System;

    class Program
    {
        static void Main()
        {
            // x の素通し = 引数と戻り値が一致 = Fucn<int, int> の方だけなのでそっちが選ばれる
            // x の型は int に
            M(x => x);

            // 明示的に double を返すと Func<int, double> の方が選ばれる
            // x の型は int に
            M(x => (double)x);

            // この場合、引数と戻り値が一致してるという条件では int なのか string なのか区別できなくてエラー
#if false
            N(x => x);
#endif
        }

        static void M(Func<int, int> x) => Console.WriteLine("int → int");
        static void M(Func<int, double> x) => Console.WriteLine("int → double");

        static void N(Func<int, int> x) => Console.WriteLine("int → int");
        static void N(Func<string, string> x) => Console.WriteLine("int → int");
    }
}
