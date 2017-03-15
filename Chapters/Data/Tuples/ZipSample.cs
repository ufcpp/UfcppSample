namespace Tuples.ZipSample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class Program
    {
        static void Main()
        {
            int[] a1 = new[] { 1, 2, 3 };
            string[] a2 = new[] { "a", "b", "c" };

            // 配列 ×2のタプルに対して、IEnumerable ×2のタプルの拡張メソッドを呼べる
            foreach (var (i, s) in (a1, a2).Zip())
            {
                Console.WriteLine($"{i}: {s}");
            }
        }
    }

    static class TupelExtensions
    {
        // IEnumerable ×2 に対する拡張メソッド
        public static IEnumerable<(T1 x1, T2 x2)> Zip<T1, T2>(this (IEnumerable<T1> items1, IEnumerable<T2> items2) t)
            => t.items1.Zip(t.items2, (x1, x2) => (x1, x2));
    }
}
