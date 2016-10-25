namespace Tuples.Deconstruction.Tuples
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        // タプルを使って2つの戻り値を返す
        static (int count, int sum) Tally(IEnumerable<int> items)
        {
            var count = 0;
            var sum = 0;
            foreach (var x in items)
            {
                sum += x;
                count++;
            }

            return (count, sum);
        }

        static void Main()
        {
            var data = new[] { 1, 2, 3, 4, 5 };
            var (count, sum) = Tally(data);
            Console.WriteLine($"{sum}/{count}");
        }
    }
}

namespace Tuples.Deconstruction.AnyType
{
    using System.Collections.Generic;

    class Program
    {
        static void Main()
        {
            var (key, value) = new KeyValuePair<string, int>("one", 1);
        }
    }

    static class Extensions
    {
        public static void Deconstruct<T, U>(this KeyValuePair<T, U> pair, out T key, out U value)
        {
            key = pair.Key;
            value = pair.Value;
        }
    }
}
