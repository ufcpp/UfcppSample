using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ConsoleApp1._01_Tuples
{
    static class Extensions
    {
        public static async Task Run()
        {
            // インデックス付き foreach
            foreach (var (x, i) in new[] { 1, 2, 3, 4, 5 }.Indexed())
            {
                Console.WriteLine($"{x}, {i}");
            }

            // 複数の Task をタプルを使って await
            await (Task.Delay(1), Task.Delay(1));
        }

        public static IEnumerable<(T item, int index)> Indexed<T>(this IEnumerable<T> items)
        {
            var i = 0;
            foreach (var x in items)
            {
                yield return (x, i);
                ++i;
            }
        }

        public static TaskAwaiter GetAwaiter(this (Task t1, Task t2) t) => Task.WhenAll(t.t1, t.t2).GetAwaiter();
    }
}