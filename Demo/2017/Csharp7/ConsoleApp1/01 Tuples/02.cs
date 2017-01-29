using System;
using System.Linq;

namespace ConsoleApp1._01_Tuples
{
    class _02
    {
        public static void Run()
        {
            var data = Enumerable.Range(0, 10000).ToArray();

            var (count, sum) = Tally(data);

            Console.WriteLine($"{count} {sum}");
        }

        private static (int count, int sum) Tally(int[] data)
        {
            var count = 0;
            var sum = 0;
            foreach (var x in data)
            {
                count++;
                sum += x;
            }

            return (count, sum);
        }
    }
}
