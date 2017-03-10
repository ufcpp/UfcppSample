using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1._02_Patterns
{
    class _05
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
                if (value is int n) sum += n;
                if (value is int[] a) sum += a.Sum();
            }

            // out 引数のところで変数に受けれる
            if (d.TryGetValue(1, out var v))
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
