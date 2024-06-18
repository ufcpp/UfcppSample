using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ValueList.Collections;

internal interface IListImpl<T>
{
    Span<T> Span { get; }
    int Position { get; set; }
    void Resize(int nextCapacity);

    public static ref T GetItem<TSelf>(scoped ref TSelf self, int index)
        where TSelf : IListImpl<T>, allows ref struct
    {
        Debug.Assert(index < self.Position);
        return ref self.Span[index];
    }

    public static void SetLength<TSelf>(scoped ref TSelf self, int length)
        where TSelf : IListImpl<T>, allows ref struct
    {
        Debug.Assert(length >= 0);
        Debug.Assert(length <= self.Span.Length);
        self.Position = length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<TSelf>(scoped ref TSelf self, T item)
        where TSelf : IListImpl<T>, allows ref struct
    {
        int pos = self.Position;

        Span<T> span = self.Span;
        if ((uint)pos < (uint)span.Length)
        {
            span[pos] = item;
            self.Position = pos + 1;
        }
        else
        {
            AddWithGrow(ref self, item);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void AddWithGrow<TSelf>(scoped ref TSelf self, T item)
        where TSelf : IListImpl<T>, allows ref struct
    {
        Debug.Assert(self.Position == self.Span.Length);
        int pos = self.Position;
        Grow(ref self, 1);
        self.Span[pos] = item;
        self.Position = pos + 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddRange<TSelf>(scoped ref TSelf self, scoped ReadOnlySpan<T> source)
        where TSelf : IListImpl<T>, allows ref struct
    {
        int pos = self.Position;
        Span<T> span = self.Span;
        if (source.Length == 1 && (uint)pos < (uint)span.Length)
        {
            span[pos] = source[0];
            self.Position = pos + 1;
        }
        else
        {
            AddRangeWithGrow(ref self, source);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void AddRangeWithGrow<TSelf>(scoped ref TSelf self, scoped ReadOnlySpan<T> source)
        where TSelf : IListImpl<T>, allows ref struct
    {
        if ((uint)(self.Position + source.Length) > (uint)self.Span.Length)
        {
            Grow(ref self, self.Span.Length - self.Position + source.Length);
        }

        source.CopyTo(self.Span[self.Position..]);
        self.Position += source.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AddSpan<TSelf>(scoped ref TSelf self, int length)
        where TSelf : IListImpl<T>, allows ref struct
    {
        Debug.Assert(length >= 0);

        int pos = self.Position;
        Span<T> span = self.Span;
        if ((ulong)(uint)pos + (ulong)(uint)length <= (ulong)(uint)span.Length)
        {
            self.Position = pos + length;
            return span.Slice(pos, length);
        }
        else
        {
            return AddSpanWithGrow(ref self, length);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Span<T> AddSpanWithGrow<TSelf>(scoped ref TSelf self, int length)
        where TSelf : IListImpl<T>, allows ref struct
    {
        int pos = self.Position;
        Grow(ref self, self.Span.Length - pos + length);
        self.Position += length;
        return self.Span.Slice(pos, length);
    }

    private static void Grow<TSelf>(scoped ref TSelf self, int additionalCapacityRequired)
        where TSelf : IListImpl<T>, allows ref struct
    {
        const int ArrayMaxLength = 0x7FFFFFC7;

        var span = self.Span;

        int nextCapacity = Math.Max(span.Length != 0 ? span.Length * 2 : 4, span.Length + additionalCapacityRequired);

        if ((uint)nextCapacity > ArrayMaxLength)
        {
            nextCapacity = Math.Max(Math.Max(span.Length + 1, ArrayMaxLength), span.Length);
        }

        self.Resize(nextCapacity);
    }
}
