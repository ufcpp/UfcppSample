using System.Collections.Generic;

namespace TaskLibrary.Channels
{
    internal static class EnumerableEx
    {
        /// <summary>
        /// 末尾に1要素追加。
        ///
        /// https://github.com/dotnet/corefx/commit/964281dfc6e1828f078890099e0042f2e5ed85bf
        /// .NET 4.6.2 で追加予定のメソッド。
        /// もし 4.6.2 未満サポートを切ってもよくなったら消す。
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
    }
}
