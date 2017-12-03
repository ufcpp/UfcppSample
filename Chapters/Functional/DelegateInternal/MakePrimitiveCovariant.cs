namespace DelegateInternal.MakePrimitiveCovariant
{
    using System;

    static class Program
    {
        static void M(Func<long> f)
        {
            Console.WriteLine(f());
        }

        static void Main()
        {
            Func<int> f = () => 1;

#if InvalidCode
            //❌コンパイル エラー。int → long へは共変性が働かないので、直接は渡せない
            M(f);
#endif

            // ラムダ式で包めば渡せる
            M(() => f());

            // 拡張メソッドでカリー化(ちょっとだけ速い)
            M(f.Upcast);
        }

        static long Upcast(this Func<int> f) => f();
    }
}
