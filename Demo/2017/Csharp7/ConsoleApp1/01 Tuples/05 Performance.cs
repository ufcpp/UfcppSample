using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ConsoleApp1._01_Tuples
{
    class Performance
    {
        // 実用の例
        // LINQのCountとかSumとかは、1つ1つループするけど、ループのオーバーヘッドが結構大きい
        // CountとSumとか、同時に集計する方が速いものを、タプルで返すように
        public static void Run()
        {
            var data = Enumerable.Range(0, 10000).ToArray();

            // 実行時間を計測
            void measure(Func<int[], (int count, int sum, double average, int min, int max)> a)
            {
                var sw = new Stopwatch();
                sw.Start();

                var r = a(data);

                sw.Stop();
                Console.WriteLine($"{sw.Elapsed} {r}");
            }

            // Count, Sum, ... を個別に、合計5ループで計算
            measure(x =>
            {
                var count = x.Count();
                var sum = x.Sum();
                var ave = x.Average();
                var min = x.Min();
                var max = x.Max();
                return (count, sum, ave, min, max);
            });

            // 1ループでまとめて計算
            measure(Tally);
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