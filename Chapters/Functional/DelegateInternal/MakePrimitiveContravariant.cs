namespace DelegateInternal.MakePrimitiveContravariant
{
    using System;

    static class Program
    {
        static void M(Action<int> a)
        {
            a(10);
        }

        static void Main()
        {
            Action<long> a = x => Console.WriteLine(x);

#if InvalidCode
            //❌コンパイル エラー。int → long へは共変性が働かないので、直接は渡せない
            M(a);
#endif

            // ラムダ式で包めば渡せる
            M(x => a(x));

            // 拡張メソッドでカリー化(ちょっとだけ速い)
            M(a.Upcast);
        }

        static void Upcast(this Action<long> a, int x) => a(x);
    }
}
