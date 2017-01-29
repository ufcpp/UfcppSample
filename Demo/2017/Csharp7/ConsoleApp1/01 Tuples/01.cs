using System;
using System.Linq;

namespace ConsoleApp1._01_Tuples
{
    class _01
    {
        public static void Run()
        {
            var data = Enumerable.Range(0, 10000).ToArray();

            var count = 0;
            var sum = 0;
            foreach (var x in data)
            {
                count++;
                sum += x;
            }

            Console.WriteLine($"{count} {sum}");
        }
    }
}
