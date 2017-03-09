using System;
using System.Collections.Generic;

namespace ConsoleApp1._03_LocalFunctions
{
    static class _02
    {
        static event EventHandler<int> SomeEvent;

        public static void Run()
        {
            // 再帰とかイベント -= とか
            // 一度 = null してから別の行で定義とかしなくてよくなる
            void handler(object sender, int arg)
            {
                Console.WriteLine("1回きりの処理");
                SomeEvent -= handler;
            }
            SomeEvent += handler;

            int factorial(int x) => x <= 1 ? 1 : x * factorial(x - 1);
        }

        static IEnumerable<(T item, int index)> Indexed<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return IndexedInternal(source);
        }

        static IEnumerable<(T item, int index)> IndexedInternal<T>(this IEnumerable<T> source)
        {
            var i = 0;
            foreach (var x in source)
            {
                yield return (x, i);
                ++i;
            }
        }
    }
}
