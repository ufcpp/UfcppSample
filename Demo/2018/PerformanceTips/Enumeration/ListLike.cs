using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Enumeration
{
    /// <summary>
    /// 列挙方式の比較用。
    /// <see cref="List{T}"/> の中身のうち、列挙に関係する部分だけ抜き出してある。
    /// 要は、長めの配列を確保して、そのうち先頭から <see cref="Length"/> だけ中身が詰まってるやつ。
    /// </summary>
    public class ListLike<T>
    {
        // 比較として生配列の列挙もやりたいので public
        // Add とかは割愛したいので、テストに使うデータも直接これに書き込んでしまうことにする。
        public T[] Array;
        public int Length;

        #region 生 Span<T> で列挙

        public Span<T> GetSpan() => Array.AsSpan(0, Length);

        #endregion
        #region List<T>.GetEnumerator に近い実装

        public struct Enumerator : IEnumerator<T>
        {
            private readonly T[] _array;
            private readonly int _length;
            private T _current;
            private int _index;

            public Enumerator(T[] array, int length) => (_array, _length, _current, _index) = (array, length, default, 0);

            public T Current => _current;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                if ((uint)_index < (uint)_length)
                {
                    _current = _array[_index];
                    _index++;
                    return true;
                }
                return false;
            }

            object IEnumerator.Current => Current;
            public void Dispose() { }
            public void Reset() => throw new NotImplementedException();
        }

        public Enumerator GetEnumerator() => new Enumerator(Array, Length);

        #endregion
        #region FastEnumerator 実装

        public struct FastEnumerator : IFastEnumerator<T>
        {
            private readonly T[] _array;
            private readonly int _length;
            private int _index;

            public FastEnumerator(T[] array, int length) => (_array, _length, _index) = (array, length, 0);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T TryMoveNext(out bool success)
            {
                if ((uint)_index < (uint)_length)
                {
                    var current = _array[_index];
                    _index++;
                    success = true;
                    return current;
                }
                success = false;
                return default;
            }
        }

        public FastEnumerator GetFastEnumerator() => new FastEnumerator(Array, Length);

        #endregion
    }
}
