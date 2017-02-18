using System;
using System.Linq;

namespace ValueTypeGenerics.GenericArithmeticOperators
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
                    var sum = Sum(items);
                    var prod = Prod(items);
                }

                var end = GC.GetTotalMemory(false);
                Console.WriteLine($"interface: {end - begin}"); // 0 にはならない
            }
        }

        public static int Sum(int[] items)
        {
            var sum = 0;
            foreach (var item in items)
                sum = sum + item;
            return sum;
        }

        public static int Prod(int[] items)
        {
            var sum = 1;
            foreach (var item in items)
                sum = sum * item;
            return sum;
        }

        static void M()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var sum = Sum(items);
            var prod = Prod(items);
        }

        static void M2()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var sum = items.Aggregate(0, (x, y) => x + y);
            var prod = items.Aggregate(0, (x, y) => x * y);
        }
    }
}
