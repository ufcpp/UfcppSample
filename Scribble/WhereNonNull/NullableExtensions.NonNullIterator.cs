using System;
using System.Collections.Generic;

namespace WhereNonNull
{
    public static partial class NullableExtensions
    {
        /// <summary>
        /// 非nullをはじく。Where(x => x.HasValue).Select(x => x.GetValueOrDefault())相当。
        /// イテレーター実装。
        /// </summary>
        public static IEnumerable<T> NonNull1<T>(this IEnumerable<T?> source)
            where T : struct
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return NonNullInternal(source);
        }

        private static IEnumerable<T> NonNullInternal<T>(IEnumerable<T?> source)
            where T : struct
        {
            foreach (var x in source)
            {
                if (x.HasValue) yield return x.Value;
            }
        }
    }
}
