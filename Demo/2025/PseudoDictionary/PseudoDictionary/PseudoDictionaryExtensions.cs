using System.Runtime.InteropServices;

namespace PseudoDictionary;

public static partial class PseudoDictionaryExtensions
{
    public static ref TValue GetValueRefOrAddDefault<TKey, TValue>(this List<(TKey Key, TValue Value)> list, TKey key, Func<TKey, TKey, bool> keyComparer)
    {
        foreach (ref var x in CollectionsMarshal.AsSpan(list))
            if (keyComparer(key, x.Key)) return ref x.Value;
        list.Add((key, default!));
        return ref CollectionsMarshal.AsSpan(list)[^1]!.Value;
    }

    public static ref TValue GetValueRefOrAddDefault<TKey, TValue>(this List<(TKey Key, TValue Value)> list, TKey key)
        where TKey : IEquatable<TKey>
    {
        foreach (ref var x in CollectionsMarshal.AsSpan(list))
            if (key.Equals(x.Key)) return ref x.Value;
        list.Add((key, default!));
        return ref CollectionsMarshal.AsSpan(list)[^1]!.Value;
    }

    public static ref TValue GetValueRefOrAddDefault<TKey, TValue>(this List<TValue> list, TKey key,
        Func<TValue, TKey> getKey,
        Func<TKey, TValue> newInstance,
        Func<TKey, TKey, bool> comparer)
    {
        foreach (ref var x in CollectionsMarshal.AsSpan(list))
            if (comparer(key, getKey(x))) return ref x;
        list.Add(newInstance(key));
        return ref CollectionsMarshal.AsSpan(list)[^1]!;
    }

    public static ref TValue GetValueRefOrAddDefault<TKey, TValue>(this List<TValue> list, TKey key,
        Func<TValue, TKey> getKey,
        Func<TKey, TValue> newInstance)
        where TKey : IEquatable<TKey>
    {
        foreach (ref var x in CollectionsMarshal.AsSpan(list))
            if (key.Equals(getKey(x))) return ref x;
        list.Add(newInstance(key));
        return ref CollectionsMarshal.AsSpan(list)[^1]!;
    }
}
