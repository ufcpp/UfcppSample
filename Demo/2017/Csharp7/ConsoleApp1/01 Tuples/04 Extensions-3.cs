using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1._01_Tuples
{
    static class Extensions3
    {
        // 実用の例。タプルを使った拡張メソッドを3つほど紹介
        // 3つ目
        public static void Run()
        {
            // 配列 ×2 なタプルに対して、IEnumerable ×2 タプルの拡張メソッドを呼べる
            var x = (new[] { 1, 2, 3 }, new[] { "a", "b", "c" }).Zip();

            foreach (var (i, s) in x)
            {
                Console.WriteLine($"{i}: {s}");
            }
        }

        // 2つの IEnumerable を Zip
        public static IEnumerable<(T x, U y)> Zip<T, U>(this (IEnumerable<T> x, IEnumerable<U> y) source) => source.x.Zip(source.y, (x, y) => (x, y));
    }
}