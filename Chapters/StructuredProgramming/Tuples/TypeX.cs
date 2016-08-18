namespace Tuples.TypeX
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        static X Measure(IEnumerable<int> items)
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

            return new X(min, max, (double)sum / count);
        }
    }

    internal struct X
    {
        public int max;
        public int min;
        public double v;

        public X(int min, int max, double v)
        {
            this.min = min;
            this.max = max;
            this.v = v;
        }
    }
}
