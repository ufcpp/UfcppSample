using System.Collections.Generic;

/// <summary>
/// 比較用。
/// <see cref="StackHashSet"/>のほぼコピペ。
/// stackalloc が new (普通に配列をヒープ確保)に変えただけ。
/// <see cref="System.Linq.Enumerable.Distinct{TSource}(IEnumerable{TSource})"/>は「配列とstackallocの比較」に使うには重たすぎるんで。
/// </summary>
static class ArrayHashSet
{
    public static T[] Distinct<T, TComp>(T[] items)
        where TComp : struct, IEqualityComparer<T>
    {
        var len = StackHashSet.PowerOf2(items.Length * 2);
        var buckets = new int[len];
        var resultIndexes = new int[items.Length];

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

                hash = (hash + StackHashSet.Skip) & mask;
            }
        }

        var results = new T[count];
        for (int i = 0; i < results.Length; i++) results[i] = items[resultIndexes[i]];
        return results;
    }
}
