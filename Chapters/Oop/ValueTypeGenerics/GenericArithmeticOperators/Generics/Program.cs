using System;

namespace ValueTypeGenerics.GenericArithmeticOperators.Interface
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
                    sum += Sum(items, default(AddOperation)); // インターフェイスを介した時点でボックス化発生

                var end = GC.GetTotalMemory(false);
                Console.WriteLine($"interface: {end - begin}"); // 0 にはならない

            }
        }

        public static T Sum<T>(T[] items, IBinaryOperator<T> op)
        {
            var sum = default(T);
            foreach (var item in items)
                sum = op.Operate(sum, item);
            return sum;
        }
    }
}
