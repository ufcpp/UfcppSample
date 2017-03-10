using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1._02_Patterns
{
    class _07
    {
        public static void Run()
        {
            var d = new Dictionary<int, object>
            {
                [1] = 1,
                [3] = new[] { 1, 2, 3 },
                [5] = null,
            };

            // 使わない値は _ で受ければ無視できる
            var (key, _) = d.First();
            Console.WriteLine(key);

            // _ なら、同じスコープに複数書いても大丈夫
            // _ は変数にならない = その後参照できない
            // ちょっと見づらいけど、 _ のところは青色(キーワードと同じ色)になってて、特別なことがわかる
            var sum = 0;
            foreach (var (_, value) in d)
            {
                if (value is int n) sum += n;
                if (value is int[] a) sum += a.Sum();
            }

            if (d.TryGetValue(1, out var v))
            {
                Console.WriteLine(v);
            }

            // out に対しても _ 利用可能
            var cd = new ConcurrentDictionary<int, int>();
            cd.TryRemove(1, out _);

            while (Console.ReadLine() is var line && !string.IsNullOrEmpty(line))
            {
                Console.WriteLine(line);
            }
        }
    }
}
