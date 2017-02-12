using System;
using System.Collections.Generic;

namespace ConsoleApp1._03_LocalFunctions
{
    static class _03
    {
        static event EventHandler<int> SomeEvent;

        public static void Run()
        {
            void handler(object sender, int arg) => SomeEvent -= handler;
            SomeEvent += handler;

            int factorial(int x) => x <= 1 ? 1 : x * factorial(x - 1);
        }

        static IEnumerable<(T item, int index)> Indexed<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            // ローカルでしか使わないものはローカルで定義
            IEnumerable<(T item, int index)> f()
            {
                var i = 0;
                foreach (var x in source)
                {
                    yield return (x, i);
                    ++i;
                }
            }

            return f();
        }
    }
}
