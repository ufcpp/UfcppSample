#pragma warning disable CS0168

namespace LocalFunctions.Lambda
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class Program
    {
        static void Main()
        {
            var input = new[] { 1, 2, 3, 4, 5 };
            var output = input
                .Where(n => n > 3)
                .Select(n => n * n);

            foreach (var x in output)
            {
                Console.WriteLine(x);
            }
        }

        static void M1()
        {
            Func<int, int> f1 =
                (int x) =>
                {
                    var sum = 0;
                    for (int i = 0; i < x; i++)
                        sum += i;
                    return sum;
                }
                ;
        }

        static void M2()
        {
            // 再帰呼び出し
            // ローカル関数は素直に書ける
            int f1(int n) => n >= 1 ? n * f1(n - 1) : 1;

            // 匿名関数はひと手間必要
            Func<int, int> f2 = null;
            f2 = n => n >= 1 ? n * f2(n - 1) : 1;

            // イテレーター
            // ローカル関数なら使える
            IEnumerable<int> g1(IEnumerable<int> items)
            {
                foreach (var x in items)
                    yield return 2 * x;
            }

#if false
            // 匿名関数では使えない(コンパイル エラーになる)
            Func<IEnumerable<int>, IEnumerable<int>> g2 = items =>
            {
                foreach (var x in items)
                    yield return 2 * x;
            }
#endif
        }
    }
}
