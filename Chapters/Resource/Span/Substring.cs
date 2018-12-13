using System;
using System.Collections.Generic;
using System.Text;

namespace Span.Substring
{
    class Program
    {
        static void Main()
        {
            var s = "abcあいう亜以宇";

            var sub = s.Substring(3, 3);
            var span = s.AsSpan().Slice(3, 3);

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine((sub[i], span[i])); // あ、い、う が2つずつ表示される
            }
        }
    }
}
