using System.Collections.Generic;

namespace CardBattle.Lib
{
    public static class EnumerableEx
    {
        /// <summary>
        /// https://github.com/dotnet/corefx/commit/964281dfc6e1828f078890099e0042f2e5ed85bf
        /// .NET 4.6.2 で追加予定のメソッド。
        /// 末尾に1要素追加。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T element)
        {
            foreach (var item in source) yield return item;
            yield return element;
        }

        /// <summary>
        /// https://github.com/dotnet/corefx/commit/964281dfc6e1828f078890099e0042f2e5ed85bf
        /// .NET 4.6.2 で追加予定のメソッド。
        /// 先頭に1要素追加。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T element)
        {
            yield return element;
            foreach (var item in source) yield return item;
        }
    }
}
