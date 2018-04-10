using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.OverloadResolution.Constraints.FirstOrNull
{
    static class ExtensionsForStruct
    {
        /// <summary>
        /// 構造体に対して <see cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/> を使うと、
        /// <paramref name="source"/> が元々 default 値を含んでいるときに困る。
        /// なので、T じゃなくて T? で null を返したいんだけど…
        ///
        /// クラス用と構造体用を同時に定義すると C# 7.2 までは呼び分けができなかった。
        /// C# 7.3 だと呼び分け可能。
        ///
        /// この手の「構造体の時だけ戻り値を T? に変えたい」需要は稀によくある。
        /// </summary>
        public static T? FirstOrNull<T>(this IEnumerable<T> source, Func<T, bool> predicate)
            where T : struct
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            foreach (var item in source)
            {
                if (predicate(item)) return item;
            }

            return null;
        }
    }

    static class ExtensionsForClass
    {
        /// <summary>
        /// クラスの場合は <see cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource}, Func{TSource, bool})/> を呼ぶだけ。
        /// </summary>
        public static T FirstOrNull<T>(this IEnumerable<T> source, Func<T, bool> predicate)
            where T : class
            => source.FirstOrDefault(predicate);
    }

    class Program
    {
        static void Main()
        {
            // ちゃんと呼び分けできているか確認

            // 構造体版が呼ばれてる。
            // 第3引数は = default が効いてて、呼び出し側では未指定で大丈夫。
            var a = new[] { 1, 2, 3, 4, 5 };
            Console.WriteLine(a.FirstOrNull(x => (x % 2) == 0)); // 2
            Console.WriteLine(a.FirstOrNull(x => x > 10)); // null (何も表示されない)

            // クラス版が呼ばれてる。
            // 第3引数は = default が効いてて、呼び出し側では未指定で大丈夫。
            var b = new[] { "", "a", "ab", "abc" };
            Console.WriteLine(b.FirstOrNull(x => x.Length > 2)); // abc
            Console.WriteLine(b.FirstOrNull(x => x.Length > 5)); // null (何も表示されない)
        }
    }
}
