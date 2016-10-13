using System;
using System.Collections;
using System.Collections.Generic;

namespace UtfString.Unsafe.Utf32
{
    public struct IndexEnumerable : IEnumerator<Index>, IEnumerable<Index>, IIndexEnumerable<Index, IndexEnumerable>
    {
        private readonly UIntAccessor _buffer;
        private Index _i;
        private bool _init;

        public IndexEnumerable(UIntAccessor buffer)
        {
            _buffer = buffer;
            _i = default(Index);
            _init = false;
        }

        public Index Current => _i;
        public bool MoveNext()
        {
            if (!_init) _init = true;
            else _i.index++;
            return _i.index < _buffer.Length;
        }

        object IEnumerator.Current => Current;
        void IDisposable.Dispose() { }
        void IEnumerator.Reset() { throw new NotSupportedException(); }

        public IndexEnumerable GetEnumerator() => new IndexEnumerable(_buffer);
        IEnumerator<Index> IEnumerable<Index>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
