namespace Tuples
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        static (int min, int max, double average) Measure(IEnumerable<int> items)
        {
            var count = 0;
            var sum = 0;
            var min = int.MaxValue;
            var max = int.MinValue;
            foreach (var x in items)
            {
                sum += x;
                count++;
                min = Math.Min(x, min);
                max = Math.Max(x, max);
            }

            return (min, max, (double)sum / count);
        }
    }
}
