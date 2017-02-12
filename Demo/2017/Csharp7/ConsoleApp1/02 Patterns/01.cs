using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1._02_Patterns
{
    class _01
    {
        public static void Run()
        {
            var d = new Dictionary<int, object>
            {
                [1] = 1,
                [3] = new[] { 1, 2, 3 },
                [5] = null,
            };

            KeyValuePair<int, object> f = d.First();
            Console.WriteLine(f.Key);

            var sum = 0;
            foreach (var x in d)
            {
                if (x.Value is int) sum += (int)x.Value;

                var a = x.Value as int[];
                if (a != null) sum += a.Sum();
            }

            object v;
            if (d.TryGetValue(1, out v))
            {
                Console.WriteLine(v);
            }
        }
    }
}
