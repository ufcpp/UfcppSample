using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1._02_Patterns
{
    class _02
    {
        public static void Run()
        {
            var d = new Dictionary<int, object>
            {
                [1] = 1,
                [3] = new[] { 1, 2, 3 },
                [5] = null,
            };

            // 分解はタプルでなくても、所定のパターンを満たしたものなら何にでも使える
            var (key, value) = d.First();
            Console.WriteLine(key);

            // ちなみに、↑は、↓みたいに展開されてる
            d.First().Deconstruct(out key, out value);

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

            string line;
            while (!string.IsNullOrEmpty((line = Console.ReadLine())))
            {
                Console.WriteLine(line);
            }
        }
    }

    static class KeyValuePairExtensions
    {
        // Deconstruct って名前のメソッドがあれば分解できる
        // ついでに: タプル+分解と、 => メソッドが結構相性いい
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> x, out TKey key, out TValue value) => (key, value) = (x.Key, x.Value);
    }
}
