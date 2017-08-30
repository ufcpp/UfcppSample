using System.Collections;
using System.Collections.Generic;
using System.Linq;

internal class FixedDictionary<TKey, TValue, TComparer> : IDictionary<TKey, TValue>
    where TComparer : struct, IEqualityComparer<TKey>
{
    private KeyValuePair<TKey, TValue>[][] table;
    private int mask => table.Length - 1;

    public bool IsNull => table == null;

    public FixedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> values)
    {
        var initialCapacity = values.Count();
        var capacity = PowerOf2(initialCapacity);

        table = new KeyValuePair<TKey, TValue>[capacity][];
        var mask = capacity - 1;

        foreach (var g in values.GroupBy(x => default(TComparer).GetHashCode(x.Key) & mask))
        {
            table[g.Key] = g.ToArray();
        }
    }

    private static int PowerOf2(int x)
    {
        var p = 1;
        while (p < x) p <<= 1;
        return p;
    }

    public TValue Get(TKey key) => TryGet(key, out var value) ? value : throw new KeyNotFoundException("Type was not dound, Type: " + key);

    public bool TryGet(TKey key, out TValue value)
    {
        var hashCode = default(TComparer).GetHashCode(key);
        var buckets = table[hashCode & mask];

        if (buckets != null)
        {
            for (int i = 0; i < buckets.Length; i++)
            {
                if (default(TComparer).Equals(buckets[i].Key, key))
                {
                    value = buckets[i].Value;
                    return true;
                }
            }
        }

        value = default(TValue);
        return false;
    }

    ICollection<TKey> IDictionary<TKey, TValue>.Keys => throw new System.NotImplementedException();
    ICollection<TValue> IDictionary<TKey, TValue>.Values => throw new System.NotImplementedException();
    int ICollection<KeyValuePair<TKey, TValue>>.Count => throw new System.NotImplementedException();
    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => true;
    TValue IDictionary<TKey, TValue>.this[TKey key] { get => Get(key); set => throw new System.NotImplementedException(); }
    bool IDictionary<TKey, TValue>.ContainsKey(TKey key) => TryGet(key, out _);
    void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => throw new System.NotImplementedException();
    bool IDictionary<TKey, TValue>.Remove(TKey key) => throw new System.NotImplementedException();
    bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) => TryGet(key, out value);
    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => throw new System.NotImplementedException();
    void ICollection<KeyValuePair<TKey, TValue>>.Clear() => throw new System.NotImplementedException();
    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => throw new System.NotImplementedException();
    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => throw new System.NotImplementedException();
    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => throw new System.NotImplementedException();
    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => table.SelectMany(x => x).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => table.SelectMany(x => x).GetEnumerator();
}
