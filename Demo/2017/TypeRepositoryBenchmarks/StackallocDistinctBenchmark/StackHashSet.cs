using System.Collections.Generic;

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
    internal static int PowerOf2(int x)
    {
        var p = 1;
        while (p < x) p <<= 1;
        return p;
    }

    internal const int Skip = 928191829;

    public static unsafe T[] Distinct<T, TComp>(this T[] items)
        where TComp : struct, IEqualityComparer<T>
    {
        var len = PowerOf2(items.Length * 2);
        var buckets = stackalloc int[len];
        var resultIndexes = stackalloc int[items.Length];

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
