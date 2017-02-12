using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1._02_Patterns
{
    class _03
    {
        public static void Run()
        {
            var d = new Dictionary<int, object>
            {
                [1] = 1,
                [3] = new[] { 1, 2, 3 },
                [5] = null,
            };

            {
                var (key, value) = d.First();
                Console.WriteLine(key);
            }

            // foreach でも分解可能
            var sum = 0;
            foreach (var (key, value) in d)
            {
                if (value is int) sum += (int)value;

                var a = value as int[];
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
