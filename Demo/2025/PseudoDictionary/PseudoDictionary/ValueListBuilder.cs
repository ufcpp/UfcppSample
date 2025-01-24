using System.Buffers;
using System.Runtime.CompilerServices;

namespace ValueList.Collections;

public ref struct ValueListBuilder<T>(Span<T> initialSpan) : IListImpl<T>
{
    private Span<T> _span = initialSpan;
    private T[]? _arrayFromPool = null;
    private int _pos = 0;

    readonly Span<T> IListImpl<T>.Span => _span;
    int IListImpl<T>.Position { readonly get => _pos; set => _pos = value; }
    void IListImpl<T>.Resize(int nextCapacity)
    {
        T[] array = ArrayPool<T>.Shared.Rent(nextCapacity);
        _span.CopyTo(array);

        T[]? toReturn = _arrayFromPool;
        _span = _arrayFromPool = array;
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
    public Span<T> AddSpan(int length) => IListImpl<T>.AddSpan(ref this, length);
    public readonly Span<T> AsSpan() => _span[.._pos];
}
