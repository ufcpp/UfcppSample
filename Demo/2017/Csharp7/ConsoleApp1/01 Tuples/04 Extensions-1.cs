using System;
using System.Collections.Generic;

namespace ConsoleApp1._01_Tuples
{
    static class Extensions1
    {
        // 実用の例。タプルを使った拡張メソッドを3つほど紹介
        // 1つ目
        public static void Run()
        {
            // インデックス付き foreach
            foreach (var (x, i) in new[] { 1, 2, 3, 4, 5 }.Indexed())
            {
                Console.WriteLine($"{x}, {i}");
            }
        }

        // インデックス付きで foreach するための拡張メソッド
        public static IEnumerable<(T item, int index)> Indexed<T>(this IEnumerable<T> items)
        {
            var i = 0;
            foreach (var x in items)
            {
                yield return (x, i);
                ++i;
            }
        }
    }
}