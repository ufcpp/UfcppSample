using System;

namespace ValueTypeGenerics.GenericArithmeticOperators.PseudoStatic
{
    class Program
    {
        static void Main()
        {
            const int N = 10000;
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            {
                var begin = GC.GetTotalMemory(false);

                for (int i = 0; i < N; i++)
                {
                    // default(T) せず、型引数だけ書く
                    var sum = Sum<int, Add>(items);
                    var prod = Sum<int, Mul>(items);
                }

                var end = GC.GetTotalMemory(false);
                Console.WriteLine($"pseudo-static: {end - begin}"); // 0 って出るはず
            }
        }

        static T Sum<T, TOperator>(T[] items)
            where TOperator : struct, IBinaryOperator<T>
        {
            var sum = default(T);
            foreach (var item in items)
                sum = default(TOperator).Operate(sum, item);
            // ↑ メソッド内で default()
            // 空の構造体なのでほぼノーコスト
            return sum;
        }

        static void M()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            // default(T) せず、型引数だけ書く
            var sum = Sum<int, Add>(items);
            var prod = Sum<int, Mul>(items);
        }
    }
}
