// static abstract メンバーをふんだんに使って、
// .NET 7 では、int とか float の + とか - とかの演算を全部インターフェイスを介して呼べるようになった。
// System.Numeric.INumber インターフェイスってのが追加されてる。

using System.Numerics;

internal static class GenericMath
{
    // いままではこんなごく単純な Sum (総和)すら、ジェネリックには書けなかった。
    public static T Sum<T>(this IEnumerable<T> source)
        where T : INumber<T>
    {
        var sum = T.Zero; // ここの Zero とか
        foreach (var x in source) sum += x; // ここの + 演算子とか
        // この辺りのメンバーが static abstract で INumeric (の親インターフェイス)に定義されてる。
        return sum;
    }

    public static void M()
    {
        // どの整数型を使っても呼べる。
        // パフォーマンスもかなりいい。
        Console.WriteLine(Sum(new byte[] { 1, 2, 3, 4, 5 }));
        Console.WriteLine(Sum(new int[] { 1, 2, 3, 4, 5 }));
        Console.WriteLine(Sum(new double[] { 1, 2, 3, 4, 5 }));
    }
}
