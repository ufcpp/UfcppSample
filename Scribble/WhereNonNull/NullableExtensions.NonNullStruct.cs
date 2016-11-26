using System;
using System.Collections;
using System.Collections.Generic;

namespace WhereNonNull
{
    public static partial class NullableExtensions
    {
        /// <summary>
        /// 非nullをはじく。Where(x => x.HasValue).Select(x => x.GetValueOrDefault())相当。
        /// イテレーターも使わず、構造体で最適化実装をやってみたもの。
        /// </summary>
        public static NonNullEnumerable<T> NonNull<T>(this IEnumerable<T?> source)
            where T : struct
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return new NonNullEnumerable<T>(source);
        }

        public struct NonNullEnumerable<T> : IEnumerable<T>
            where T : struct
        {
            IEnumerable<T?> _source;
            internal NonNullEnumerable(IEnumerable<T?> source){_source = source;}
            public NonNullEnumerator<T> GetEnumerator() => new NonNullEnumerator<T>(_source.GetEnumerator());

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        }

        public struct NonNullEnumerator<T> : IEnumerator<T>
            where T : struct
        {
            IEnumerator<T?> _e;
            internal NonNullEnumerator(IEnumerator<T?> e) { _e = e; Current = default(T); }

            public T Current { get; private set; }

            public bool MoveNext()
            {
                while(_e.MoveNext())
                {
                    var c = _e.Current;
                    if(c.HasValue)
                    {
                        Current = c.GetValueOrDefault();
                        return true;
                    }
                }
                return false;
            }

            object IEnumerator.Current => _e.Current;

            T IEnumerator<T>.Current
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            void IDisposable.Dispose() { }
            void IEnumerator.Reset() { throw new NotImplementedException(); }
        }
    }
}
