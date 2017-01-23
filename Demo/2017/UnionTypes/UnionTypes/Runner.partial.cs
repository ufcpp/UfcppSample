using System;
using System.Diagnostics;

namespace UnionTypes
{
    partial class Runner
    {
        const int NumLoop = 1000000;

        public static TimeSpan Run<T, Union>(string message, double arrayRate, Random r, Func<Random, double, Union> generator)
            where Union : IUnion<T>
        {
            GC.Collect();

            int arrayCount = 0;
            int valueCount = 0;
            int itemCount = 0;

            var sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < NumLoop; i++)
            {
                var x = generator(r, arrayRate);
                var a = x.Array;
                if (a != null)
                {
                    arrayCount++;
                    itemCount += a.Length;
                }
                else
                {
                    valueCount++;
                    itemCount++;
                }
            }

            sw.Stop();
            //Console.WriteLine($"{message}: {sw.Elapsed}");
            //Console.WriteLine($"{arrayCount} {valueCount} {itemCount} {arrayCount + valueCount}");
            //Console.WriteLine();

            return sw.Elapsed;
        }
    }
}
