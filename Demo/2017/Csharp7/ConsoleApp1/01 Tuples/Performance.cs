using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ConsoleApp1._01_Tuples
{
    class Performance
    {
        public static void Run()
        {
            var data = Enumerable.Range(0, 10000).ToArray();

            var sw = new Stopwatch();
            sw.Start();

            var count = data.Count();
            var sum = data.Sum();
            var ave = data.Average();
            var min = data.Min();
            var max = data.Max();

            sw.Stop();
            Console.WriteLine($"{sw.Elapsed} {(count, sum, ave, min, max)}");

            sw.Restart();

            var t = Tally(data);

            sw.Stop();
            Console.WriteLine($"{sw.Elapsed} {t}");
        }

        static (int count, int sum, double average, int min, int max) Tally(IEnumerable<int> data)
        {
            var count = 0;
            var sum = 0;
            var min = int.MaxValue;
            var max = int.MinValue;

            foreach (var x in data)
            {
                count++;
                sum += x;
                if (min > x) min = x;
                if (max < x) max = x;
            }

            return (count, sum, sum / (double)count, min, max);
        }
    }
}