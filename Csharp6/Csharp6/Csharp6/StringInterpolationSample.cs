using System;
using System.Collections.Generic;

namespace Csharp6.Csharp6
{
    class StringInterpolationSample
    {
        void X(int x, int y)
        {
            var formatted = $"{x}, {y})";
        }

        static void Y()
        {
            IEnumerable<string> Cultures = new[] { "en-us", "fr", "zh-hk", "ja-jp" };

            var x = 10;

            IFormattable f = $"{x:c}, {x:n}";

            foreach (var c in Cultures)
            {
                Console.WriteLine(f.ToString(null, new System.Globalization.CultureInfo(c)));
            }
        }

        void Z(int x, int y)
        {
            var formatted = $@"
verbatim (here) formattable string
{x}, {y}
";
        }
    }
}
