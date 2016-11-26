using System;
using System.Collections;
using System.Collections.Generic;

namespace WhereNonNull
{
    public static partial class NullableExtensions
    {
        /// <summary>
        /// <see cref="Nullable{T}"/>は、長さが0か1の<see cref="IEnumerable{T}"/>である。
        /// </summary>
        public static NullableEnumerable<T> AsEnumerable<T>(this T? item) where T : struct => new NullableEnumerable<T>(item);

        public struct NullableEnumerable<T> : IEnumerable<T>
            where T : struct
        {
            T? _item;
            public NullableEnumerable(T? item) { _item = item; }

            NullableEnumerator<T> GetEnumerator() => new NullableEnumerator<T>(_item);
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        }

        public struct NullableEnumerator<T> : IEnumerator<T>
            where T : struct
        {
            bool _hasValue;
            public NullableEnumerator(T? value)
            {
                _hasValue = value.HasValue;
                Current = value.GetValueOrDefault();
            }

            public T Current { get; private set; }

            public bool MoveNext()
            {
                var temp = _hasValue;
                _hasValue = false;
                return temp;
            }

            object IEnumerator.Current => Current;
            void IDisposable.Dispose() { }
            void IEnumerator.Reset() { throw new NotImplementedException(); }
        }
    }
}
