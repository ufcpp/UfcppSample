using System.Collections.Generic;
using System.Runtime.CompilerServices;

/// <summary>
/// <see cref="ArrayHashSet"/>をstackallocで実装したもの。
///
/// 以下の前提で最適化実装した<see cref="System.Linq.Enumerable.Distinct{TSource}(IEnumerable{TSource})"/>。
/// - (配列とか)最初からシーケンスの長さが分かってる
/// - そんなに長くない(せいぜい数百～千程度)ことが分かってる
/// - 遅延評価も要らない(結果の格納用にその場で配列を作る)
///
/// <see cref="ArrayHashSet"/>の方も同じ前提で同じロジックで実装したものなんだけど、
/// こっちはさらに「一時バッファーをstackallocで取る」って最適化をしたもの。
/// stackallocなのでそんなに長いシーケンスに対しては使えないけど、長さによって配列かstackallocかを切り替えるような処理を入れれば実用に足ると思う。
///
/// 実用化するなら:
/// - 入力の長さによって new か stackalloc を切り替える(あんまり長いものに対して stackalloc するとスタックオーバーフローの原因になる)
/// - 引数の型は <see cref="IReadOnlyList{T}"/> の方がいいかも
/// - C# 7.2 で、Span を使えば safe にできる & 配列とstackallocで共通ロジックを使える
/// </summary>
static class StackHashSet
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int PowerOf2(int x)
    {
        var p = 1;
        while (p < x) p <<= 1;
        return p;
    }

    private static unsafe T[] ToArray<T>(T[] items, int* resultIndexes, int count)
    {
        var results = new T[count];
        for (int i = 0; i < results.Length; i++) results[i] = items[resultIndexes[i]];
        return results;
    }

    public static unsafe T[] Distinct<T, TComp>(this T[] items)
        where TComp : struct, IEqualityComparer<T>
    {
        var len = PowerOf2(items.Length * 2);
        var buckets = stackalloc int[len];
        var set = new StackHashSet<T, TComp>(items, buckets, len);

        var resultIndexes = stackalloc int[items.Length];
        var count = 0;

        for (int i = 0; i < items.Length; i++)
        {
            if (!set.Add(items[i], i))
            {
                resultIndexes[count++] = i;
            }
        }

        return ToArray(items, resultIndexes, count);
    }

    public static unsafe T[] Except<T, TComp>(this T[] first, T[] second)
        where TComp : struct, IEqualityComparer<T>
    {
        var len = PowerOf2(second.Length * 2);
        var buckets = stackalloc int[len];
        var set = new StackHashSet<T, TComp>(second, buckets, len);

        set.MakeHashtTable();

        var resultIndexes = stackalloc int[first.Length];
        var count = 0;

        for (int i = 0; i < first.Length; i++)
        {
            if (!set.Contains(first[i]))
            {
                resultIndexes[count++] = i;
            }
        }

        return ToArray(first, resultIndexes, count);
    }
}

unsafe struct StackHashSet<T, TComp>
    where TComp : struct, IEqualityComparer<T>
{
    internal const int Skip = 928191829;

    private T[] items;
    private int* buckets;
    int len;
    int mask;

    public StackHashSet(T[] items, int* buckets, int len)
    {
        this.items = items;
        this.buckets = buckets;
        this.len = len;
        mask = len - 1;

        for (int i = 0; i < len; i++) buckets[i] = -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(T item)
    {
        var hash = default(TComp).GetHashCode(item) & mask;
        while (true)
        {
            ref var b = ref buckets[hash];

            if (b == -1)
            {
                return false;
            }
            else if (default(TComp).Equals(item, items[b]))
            {
                return true;
            }

            hash = (hash + Skip) & mask;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MakeHashtTable()
    {
        var mask = len - 1;
        for (int i = 0; i < items.Length; i++)
        {
            Add(items[i], i);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Add(T item, int i)
    {
        var hash = default(TComp).GetHashCode(item) & mask;

        while (true)
        {
            ref var b = ref buckets[hash];

            if (b == -1)
            {
                b = i;
                return false;
            }
            else if (default(TComp).Equals(item, items[b]))
            {
                return true;
            }

            hash = (hash + Skip) & mask;
        }
    }
}