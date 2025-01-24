using ValueList.Collections;

namespace PseudoDictionary;

public static partial class PseudoDictionaryExtensions
{
    public static ref TValue GetValueRefOrAddDefault<TKey, TValue>(this ref ValueListBuilder<(TKey Key, TValue Value)> list, TKey key, Func<TKey, TKey, bool> keyComparer)
    {
        foreach (ref var x in list.AsSpan())
            if (keyComparer(key, x.Key)) return ref x.Value;
        list.Add((key, default!));
        return ref list.AsSpan()[^1]!.Value;
    }

    public static ref TValue GetValueRefOrAddDefault<TKey, TValue>(this ref ValueListBuilder<(TKey Key, TValue Value)> list, TKey key)
        where TKey : IEquatable<TKey>
    {
        foreach (ref var x in list.AsSpan())
            if (key.Equals(x.Key)) return ref x.Value;
        list.Add((key, default!));
        return ref list.AsSpan()[^1]!.Value;
    }

    public static ref TValue GetValueRefOrAddDefault<TKey, TValue>(this ref ValueListBuilder<TValue> list, TKey key,
        Func<TValue, TKey> getKey,
        Func<TKey, TValue> newInstance,
        Func<TKey, TKey, bool> comparer)
    {
        foreach (ref var x in list.AsSpan())
            if (comparer(key, getKey(x))) return ref x;
        list.Add(newInstance(key));
        return ref list.AsSpan()[^1]!;
    }

    public static ref TValue GetValueRefOrAddDefault<TKey, TValue>(this ref ValueListBuilder<TValue> list, TKey key,
        Func<TValue, TKey> getKey,
        Func<TKey, TValue> newInstance)
        where TKey : IEquatable<TKey>
    {
        foreach (ref var x in list.AsSpan())
            if (key.Equals(getKey(x))) return ref x;
        list.Add(newInstance(key));
        return ref list.AsSpan()[^1]!;
    }
}
