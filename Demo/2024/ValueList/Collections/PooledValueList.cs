using System.Buffers;
using System.Runtime.CompilerServices;

namespace ValueList.Collections;

public struct PooledValueList<T> : IListImpl<T>, IDisposable
{
    private T[]? _arrayFromPool;
    private int _pos;

    readonly Span<T> IListImpl<T>.Span => _arrayFromPool;
    int IListImpl<T>.Position { readonly get => _pos; set => _pos = value; }
    void IListImpl<T>.Resize(int nextCapacity)
    {
        T[] array = ArrayPool<T>.Shared.Rent(nextCapacity);
        _arrayFromPool.AsSpan().CopyTo(array);

        T[]? toReturn = _arrayFromPool;
        _arrayFromPool = array;
        if (toReturn != null)
        {
            ArrayPool<T>.Shared.Return(toReturn);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        T[]? toReturn = _arrayFromPool;
        if (toReturn != null)
        {
            _arrayFromPool = null;
            ArrayPool<T>.Shared.Return(toReturn);
        }
    }

    public int Length { readonly get => _pos; set => IListImpl<T>.SetLength(ref this, value); }
    public ref T this[int index] => ref IListImpl<T>.GetItem(ref this, index);
    public void Add(T item) => IListImpl<T>.Add(ref this, item);
    public void AddRange(scoped ReadOnlySpan<T> source) => IListImpl<T>.AddRange(ref this, source);
    public Span<T> AddSpan(int length) => IListImpl<T>.AddSpan(ref this, length);
    public readonly ReadOnlySpan<T> AsSpan() => _arrayFromPool.AsSpan()[.._pos];
    public readonly ReadOnlyMemory<T> AsMemory() => _arrayFromPool.AsMemory()[.._pos];
}
