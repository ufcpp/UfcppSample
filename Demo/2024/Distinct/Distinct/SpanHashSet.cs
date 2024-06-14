using System.Runtime.CompilerServices;

namespace Distinct;

public readonly ref struct SpanHashSet<T, TKey>(Span<T> buffer, Func<T, TKey> getKey, Func<T, bool> isDefault)
    where TKey : notnull
{
    private readonly Span<T> _buffer = buffer;

    public bool Add(T item)
    {
        ref var r = ref GetOrAddValueRef(getKey(item));
        if (!isDefault(r)) return false;

        r = item;
        return true;
    }

    public ref T GetOrAddValueRef(TKey key)
    {
        var buffer = _buffer;
        var len = buffer.Length;

        int collisionCount = 0;
        int bucketIndex = key.GetHashCode() % len;
        for (int i = bucketIndex; i < len; i = (i + 1) % len)
        {
            ref var item = ref buffer[i];
            if (key.Equals(getKey(item))
                || isDefault(item))
                return ref item!;

            if (collisionCount == buffer.Length) Throw();

            collisionCount++;
        }

        Throw();
        return ref Unsafe.NullRef<T>();
    }

    private static void Throw() => throw new InvalidOperationException();

    public int Compact()
    {
        var buffer = _buffer;
        var len = buffer.Length;

        var count = 0;
        for (int i = 0; i < len; i++)
        {
            ref var item = ref buffer[i];
            if (!isDefault(item))
            {
                if (count != i)
                {
                    buffer[count] = item;
                    item = default!;
                }

                count++;
            }
        }

        return count;
    }
}

