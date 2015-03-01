using System;
using System.Collections.Generic;

namespace Csharp6.Csharp5
{
    class StringInterpolationSample
    {
        void X(int x, int y)
        {
            var formatted = string.Format("({0}, {1})", x, y);
        }

        static void Y()
        {
            IEnumerable<string> Cultures = new[] { "en-us", "fr", "zh-hk", "ja-jp" };

            var x = 10;

            IFormattable f = System.Runtime.CompilerServices.FormattableStringFactory.Create("{0:c}, {1:n}", x, x);

            foreach (var c in Cultures)
            {
                Console.WriteLine(f.ToString(null, new System.Globalization.CultureInfo(c)));
            }
        }

        void Z(int x, int y)
        {
            var formatted = string.Format(@"
verbatim (here) formattable string
{0}, {1}
", x, y);
        }
    }
}
