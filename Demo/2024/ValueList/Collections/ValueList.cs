namespace ValueList.Collections;

public struct ValueList<T> : IListImpl<T>
{
    private T[]? _array;
    private int _pos;

    readonly Span<T> IListImpl<T>.Span => _array;
    int IListImpl<T>.Position { readonly get => _pos; set => _pos = value; }
    void IListImpl<T>.Resize(int nextCapacity) => Array.Resize(ref _array, nextCapacity);

    public int Length { readonly get => _pos; set => IListImpl<T>.SetLength(ref this, value); }
    public ref T this[int index] => ref IListImpl<T>.GetItem(ref this, index);
    public void Add(T item) => IListImpl<T>.Add(ref this, item);
    public void AddRange(scoped ReadOnlySpan<T> source) => IListImpl<T>.AddRange(ref this, source);
    public Span<T> AddSpan(int length) => IListImpl<T>.AddSpan(ref this, length);
    public readonly ReadOnlySpan<T> AsSpan() => _array.AsSpan()[.._pos];
    public readonly ReadOnlyMemory<T> AsMemory() => _array.AsMemory()[.._pos];
}
