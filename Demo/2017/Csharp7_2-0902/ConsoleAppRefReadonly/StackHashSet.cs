using System;
using System.Collections.Generic;

namespace ConsoleAppRefReadonly
{
    static class StackHashSet
    {
        internal static int PowerOf2(int x)
        {
            var p = 1;
            while (p < x) p <<= 1;
            return p;
        }

        internal const int Skip = 928191829;

        public static T[] Distinct<T, TComp>(this T[] items)
            where TComp : struct, IEqualityComparer<T>
        {
            var len = PowerOf2(items.Length * 2);
            Span<int> buckets = len <= 1024 ? (Span<int>)stackalloc int[len] : (Span<int>)new int[len];
            Span<int> resultIndexes = len <= 1024 ? (Span<int>)stackalloc int[items.Length] : (Span<int>)new int[items.Length];

            for (int i = 0; i < len; i++) buckets[i] = -1;

            var mask = len - 1;
            var count = 0;

            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];

                var hash = default(TComp).GetHashCode(item) & mask;

                while (true)
                {
                    ref var b = ref buckets[hash];

                    if (b == -1)
                    {
                        resultIndexes[count] = i;
                        count++;
                        b = i;
                        break;
                    }
                    else if (default(TComp).Equals(item, items[b]))
                    {
                        break;
                    }

                    hash = (hash + Skip) & mask;
                }
            }

            var results = new T[count];
            for (int i = 0; i < results.Length; i++) results[i] = items[resultIndexes[i]];
            return results;
        }
    }
}
