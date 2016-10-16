using System;

namespace ValueTypeGenerics.GenericArithmeticOperators.Generics
{
    class GenericArithmeticOperators
    {
        static void Main()
        {
            const int N = 10000;
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            {
                var begin = GC.GetTotalMemory(false);

                var sum = 0;
                for (int i = 0; i < N; i++)
                    sum += Sum<int, AddOperation>(items); // ジェネリックを介せばボックス化を避けれる

                var end = GC.GetTotalMemory(false);
                Console.WriteLine($"generics: {end - begin}"); // 0 って出るはず

            }
        }

        public static T Sum<T, TOperator>(T[] items)
            where TOperator : struct, IBinaryOperator<T>
        {
            var sum = default(T);
            foreach (var item in items)
                sum = default(TOperator).Operate(sum, item); // 空の構造体なのでほぼノーコスト
            return sum;
        }
    }
}
