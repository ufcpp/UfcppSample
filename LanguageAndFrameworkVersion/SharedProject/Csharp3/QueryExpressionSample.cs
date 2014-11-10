#if Ver2Plus

using System;
using System.Linq;

namespace VersionSample.Csharp3
{
    public class QueryExpressionSample
    {
        public void Run()
        {
            var input = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var output =
                from x in input
                where (x % 2) == 1
                select x * x;

            foreach (var x in output)
            {
                Console.WriteLine(x);
            }
        }
    }
}

#endif
