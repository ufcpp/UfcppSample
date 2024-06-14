namespace Distinct;

public class Hash
{
    /// <summary>
    /// かなり癖の強い実装。
    /// </summary>
    /// <remarks>
    /// * buffer がデフォルト値で初期化されてないと動作しない
    /// * source にデフォルト値が入ってくると動作しない
    /// * buffer サイズが source の倍程度の長さがないと衝突多くて遅い
    /// </remarks>
    public static Span<T> Distinct<T, TKey>(IEnumerable<T> source, Span<T> buffer, Func<T, TKey> getKey, Func<T, bool> isDefault)
        where TKey : notnull
    {
        SpanHashSet<T, TKey> set = new(buffer, getKey, isDefault);

        foreach (var x in source) set.Add(x);

        var len = set.Compact();
        return buffer[..len];
    }
}
