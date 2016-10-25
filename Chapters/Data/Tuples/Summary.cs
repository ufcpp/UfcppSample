namespace Tuples.Summary
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
            var t = Tally(data);
            Console.WriteLine($"{t.sum}/{t.count}");
        }
    }
}
