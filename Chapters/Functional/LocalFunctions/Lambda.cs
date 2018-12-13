#pragma warning disable CS0168
#pragma warning disable CS8321

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

        static void 再帰()
        {
            // 再帰呼び出し
            // ローカル関数は素直に書ける
            int f1(int n) => n >= 1 ? n * f1(n - 1) : 1;

            // 匿名関数はひと手間必要
            Func<int, int> f2 = null;
            f2 = n => n >= 1 ? n * f2(n - 1) : 1;
        }

        static void イテレーター()
        {
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

        static void ジェネリック()
        {
            // ジェネリック(polymorphic lambda)
            // ローカル関数ならジェネリックに使える
            bool eq1<T>(T x, T y) where T : IComparable<T> => x.CompareTo(y) == 0;
            Console.WriteLine(eq1(1, 2));
            Console.WriteLine(eq1("aaa", "aaa"));

#if false
            // 匿名関数はジェネリックにならない
            // Func<T, T, bool> の時点でコンパイル エラー
            // where 制約を付ける構文もない
            Func<T, T, bool> eq2 = (x, y) => x.CompareTo(y) == 0;
            // 当然、呼べない
            Console.WriteLine(eq2(1, 2));
            Console.WriteLine(eq2("aaa", "aaa"));
#endif
        }

        static void 引数の規定値()
        {
            // ローカル関数の引数には規定値を与えられる
            int f1(int n = 0) => 2 * n;
            Console.WriteLine(f1());
            Console.WriteLine(f1(5));

#if false
            // 匿名関数は無理
            Func<int, int> f2 = (x, y) => x.CompareTo(y) == 0;
            // 当然、呼べない
            Console.WriteLine(eq2(1, 2));
            Console.WriteLine(eq2("aaa", "aaa"));
#endif
        }
    }
}
