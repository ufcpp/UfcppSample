using System;
using System.Collections.Generic;

namespace ConsoleApp1._03_LocalFunctions
{
    // C# Interactiveのついなので、ローカル関数の導入自体は割と低コスト
    // ラムダ式に対して出ていた要望のいくつかが、ローカル関数なら解決できる

    static class _01
    {
        static event EventHandler<int> SomeEvent;

        public static void Run()
        {
            // イベントを1回限り受け取る
            EventHandler<int> handler = null;
            handler = (sender, arg) =>
            {
                SomeEvent -= handler;
            };
            SomeEvent += handler;

            // 再帰呼び出し
            Func<int, int> factorial = null;
            factorial = x => x <= 1 ? 1 : x * factorial(x - 1);
        }

        static IEnumerable<(T item, int index)> Indexed<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return IndexedInternal(source);
        }

        // Indexed からしか呼ばないから隠したい
        // でも、ラムダ式では yield return が使えない
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
