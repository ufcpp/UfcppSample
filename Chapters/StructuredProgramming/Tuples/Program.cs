using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuples
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }

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
