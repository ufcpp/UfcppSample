using System.Collections.Generic;
using System.Linq;

namespace A
{
    /// <summary>
    /// Linq-like.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// doubles a sequence.
        /// </summary>
        /// <typeparam name="T">type of elements</typeparam>
        /// <param name="source">source sequence</param>
        /// <returns>result sequence</returns>
        public static IEnumerable<T> Double<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new System.ArgumentNullException(nameof(source));
            }

            return source.Concat(source);
        }
    }
}
