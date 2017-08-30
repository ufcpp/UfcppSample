using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// <see cref="FixedDictionary{TKey, TValue, TComparer}"/>と性能差ほとんどなし。
/// 向こうは2次元配列
/// こっちは1次元配列なので、こっちの方がたぶんGCにやさしい。
/// </summary>
internal struct FixedDictionary2<TKey, TValue, TComparer> : IDictionary<TKey, TValue>
    where TComparer : struct, IEqualityComparer<TKey>
{
    private struct Bucket
    {
        public bool HasValue;
        public TKey Key;
        public TValue Value;
    }

    private Bucket[] _buckets;

    public bool IsNull => _buckets == null;
    private const int Skip = 655883; // 適当な大き目の素数(たぶん、奇数なら何でもいいはず)

    public FixedDictionary2(IEnumerable<KeyValuePair<TKey, TValue>> values)
    {
        var initialCapacity = values.Count() * 2;
        var capacity = PowerOf2(initialCapacity);

        _buckets = new Bucket[capacity];
        var mask = capacity - 1;

        foreach (var x in values)
        {
            var hash = default(TComparer).GetHashCode(x.Key) & mask;

            while (true)
            {
                ref var b = ref _buckets[hash];

                if (!b.HasValue)
                {
                    b.HasValue = true;
                    b.Key = x.Key;
                    b.Value = x.Value;
                    break;
                }

                hash = (hash + Skip) % mask;
            }
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
        var mask = _buckets.Length - 1;
        var hash = default(TComparer).GetHashCode(key) & mask;

        while (true)
        {
            ref var b = ref _buckets[hash];

            if (!b.HasValue)
            {
                value = default;
                return false;
            }
            else if (default(TComparer).Equals(b.Key, key))
            {
                value = b.Value;
                return true;
            }

            hash = (hash + Skip) % mask;
        }
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
    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => _buckets.Where(x => x.HasValue).Select(x => new KeyValuePair<TKey, TValue>(x.Key, x.Value)).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<KeyValuePair<TKey, TValue>>)this).GetEnumerator();
}
