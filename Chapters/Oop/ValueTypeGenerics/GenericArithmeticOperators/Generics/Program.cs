using System;

namespace ValueTypeGenerics.GenericArithmeticOperators.Generics
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
                    // ジェネリックを介せばボックス化を避けれる
                    var sum = Sum(items, default(Add));
                    var prod = Sum(items, default(Mul));
                }

                var end = GC.GetTotalMemory(false);
                Console.WriteLine($"interface: {end - begin}"); // 0 にはならない
            }
        }

        public static T Sum<T, TOperator>(T[] items, TOperator op)
            where TOperator : struct, IBinaryOperator<T>
        {
            var sum = op.Zero;
            foreach (var item in items)
                sum = op.Operate(sum, item);
            return sum;
        }

        static void M()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            // ジェネリックを介せばボックス化を避けれる
            var sum = Sum(items, default(Add));
            var prod = Sum(items, default(Mul));
        }
    }
}
