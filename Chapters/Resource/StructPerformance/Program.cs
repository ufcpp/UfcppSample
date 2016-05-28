using System;
using System.Diagnostics;

namespace StructPerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            const int count = 5 * 1000 * 1000;

            var sm = new Struct.Mutable.Calculator();
            var si = new Struct.Immutable.Calculator();
            var cm = new Class.Mutable.Calculator();
            var ci = new Class.Immutable.Calculator();

            for (int i = 0; i < 10; i++)
            {
                var a = Run(sm, count, i);
                var b = Run(si, count, i);
                var c = Run(cm, count, i);
                var d = Run(ci, count, i);
                Console.WriteLine();

                if (!MemberwiseEquals(a, b) || !MemberwiseEquals(b, c) || !MemberwiseEquals(c, d))
                    throw new InvalidOperationException();
            }
        }
        public static T Run<T>(ICalculator<T> c, int count, int randomSeed)
        {
            var sw = new Stopwatch();
            var r = new Random(randomSeed);

            sw.Start();

            var series = c.GetSeries(r, count);

            sw.Stop();
            var init = sw.Elapsed;

            sw.Restart();

            var sum = c.SeriesSum(series);

            sw.Stop();
            var calc = sw.Elapsed;

            Console.WriteLine($"{c.Name} / {init.TotalSeconds: 0.0000} / {calc.TotalSeconds: 0.0000}");

            return sum;
        }

        static bool MemberwiseEquals(dynamic a, dynamic b)
            => (double)a.A == (double)b.A
            && (double)a.B == (double)b.B
            && (double)a.C == (double)b.C
            && (double)a.D == (double)b.D
            && (double)a.E == (double)b.E
            && (double)a.F == (double)b.F
            && (double)a.G == (double)b.G
            && (double)a.H == (double)b.H;
    }
}
