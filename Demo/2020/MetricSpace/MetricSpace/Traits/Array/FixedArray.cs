using System;
using System.Runtime.CompilerServices;

namespace MetricSpace
{
    // consider using T4 template if you want higher dimensions.

    public struct Fixed1<T> : IFixedArrayAccessor<T, Fixed1<T>.Array>
    {
        public struct Array : IFixedArray<T>
        {
            public T Item1;
            public Array(T item1) => Item1 = item1;
            public static implicit operator Array(T value) => new Array(value);
            public override string ToString() => $"({Item1})";
        }

        public Array New() => default;
        public int Length => 1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe Span<T> AsSpan(ref Array array) => new Span<T>(Unsafe.AsPointer(ref Unsafe.As<Array, T>(ref array)), 1);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T At(ref Array array, int i) => ref AsSpan(ref array)[i];
    }

    public struct Fixed2<T> : IFixedArrayAccessor<T, Fixed2<T>.Array>
    {
        public struct Array : IFixedArray<T>
        {
            public T Item1; public T Item2;
            public Array(T item1, T item2) => (Item1, Item2) = (item1, item2);
            public static implicit operator Array((T, T) value) => new Array(value.Item1, value.Item2);
            public override string ToString() => $"({Item1}, {Item2})";
        }

        public Array New() => default;
        public int Length => 2;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe Span<T> AsSpan(ref Array array) => new Span<T>(Unsafe.AsPointer(ref Unsafe.As<Array, T>(ref array)), 2);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T At(ref Array array, int i) => ref AsSpan(ref array)[i];
    }

    public struct Fixed3<T> : IFixedArrayAccessor<T, Fixed3<T>.Array>
    {
        public struct Array : IFixedArray<T>
        {
            public T Item1; public T Item2; public T Item3;
            public Array(T item1, T item2, T item3) => (Item1, Item2, Item3) = (item1, item2, item3);
            public static implicit operator Array((T, T, T) value) => new Array(value.Item1, value.Item2, value.Item3);
            public override string ToString() => $"({Item1}, {Item2}, {Item3})";
        }

        public Array New() => default;
        public int Length => 3;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe Span<T> AsSpan(ref Array array) => new Span<T>(Unsafe.AsPointer(ref Unsafe.As<Array, T>(ref array)), 3);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T At(ref Array array, int i) => ref AsSpan(ref array)[i];
    }

    public struct Fixed4<T> : IFixedArrayAccessor<T, Fixed4<T>.Array>
    {
        public struct Array : IFixedArray<T>
        {
            public T Item1; public T Item2; public T Item3; public T Item4;
            public Array(T item1, T item2, T item3, T item4) => (Item1, Item2, Item3, Item4) = (item1, item2, item3, item4);
            public static implicit operator Array((T, T, T, T) value) => new Array(value.Item1, value.Item2, value.Item3, value.Item4);
            public override string ToString() => $"({Item1}, {Item2}, {Item3}, {Item4})";
        }

        public Array New() => default;
        public int Length => 4;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe Span<T> AsSpan(ref Array array) => new Span<T>(Unsafe.AsPointer(ref Unsafe.As<Array, T>(ref array)), 4);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T At(ref Array array, int i) => ref AsSpan(ref array)[i];
    }

    public static class FixedArray<T, TArray, TArrayAccessor>
        where T : IEquatable<T>
        where TArray : struct, IFixedArray<T>
        where TArrayAccessor : struct, IFixedArrayAccessor<T, TArray>
    {
        private static readonly TArrayAccessor Accessor = default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(TArray x, TArray y)
        {
            if (Accessor.Length == 2) return Equals2(x, y);
            if (Accessor.Length == 1) return Equals1(x, y);
            return EqualsMore(ref x, ref y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool Equals1(TArray x, TArray y)
            => Accessor.At(ref x, 0).Equals(Accessor.At(ref y, 0));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool Equals2(TArray x, TArray y)
            => Accessor.At(ref x, 0).Equals(Accessor.At(ref y, 0))
            && Accessor.At(ref x, 1).Equals(Accessor.At(ref y, 1));

        private static bool EqualsMore(ref TArray x, ref TArray y)
        {
            for (int i = 0; i < Accessor.Length; i++)
            {
                if (!Accessor.At(ref x, i).Equals(Accessor.At(ref y, i))) return false;
            }

            return true;
        }
    }
}
