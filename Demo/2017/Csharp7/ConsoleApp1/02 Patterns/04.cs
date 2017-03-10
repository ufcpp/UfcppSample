using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1._02_Patterns
{
    class _04
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

            var sum = 0;
            foreach (var (key, value) in d)
            {
                // is の後ろで変数に受けれる
                if (value is int n) sum += n;
                if (value is int[] a) sum += a.Sum();

                // switch も可
#if false
                switch (value)
                {
                    case int n: sum += n;
                    case int[] a: sum += a.Sum();
                }
#endif
            }

            object v;
            if (d.TryGetValue(1, out v))
            {
                Console.WriteLine(v);
            }

            string line;
            while (!string.IsNullOrEmpty((line = Console.ReadLine())))
            {
                Console.WriteLine(line);
            }
        }
    }
}
