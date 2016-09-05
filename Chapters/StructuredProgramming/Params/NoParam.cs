namespace Params.NoParam
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var x = Sum();

#if false
            // .NET Framework 4.5 以前はこういう扱い
            var x = Sum(new int[0]);
#elif false
            // .NET Framework 4.6 移行はこういう扱い
            var x = Sum(Array.Empty<int>());
#endif

            Console.WriteLine(x); // 0
        }

        static int Sum(params int[] source)
        {
            // 引数なしで呼ばれた場合、source には空配列が入る
            // source が null にはならない
            var sum = 0;
            foreach (var x in source) sum += x;
            return sum;
        }
    }
}

namespace Params.NoParam.BrakingChange
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var x = IsCached();
            Console.WriteLine(x);
            var y = IsCached();
            Console.WriteLine(y); // ターゲットによって結果が変わる
        }

        static int[] prev;

        static bool IsCached(params int[] source)
        {
            // .NET 4.5 以前だと、毎回違う配列がnewされて渡ってくる
            // .NET 4.6 移行だと、毎回同じインスタンスが使いまわされる
            if (prev == source) return true;

            prev = source;
            return false;
        }
    }
}
