using System;
using System.Collections;
using System.Collections.Generic;

namespace UtfString.Utf8
{
    public struct IndexEnumerable : IEnumerator<Index>, IEnumerable<Index>
    {
        private readonly byte[] _buffer;
        private Index _i;

        public IndexEnumerable(byte[] buffer)
        {
            _buffer = buffer;
            _i = default(Index);
        }

        public Index Current => _i;
        public bool MoveNext()
        {
            _i.index += _i.byteCount;
            return Decoder.TryGetByteCount(_buffer, _i.index, out _i.byteCount);
        }

        object IEnumerator.Current => Current;
        void IDisposable.Dispose() { }
        public void Reset() { throw new NotSupportedException(); }

        public IndexEnumerable GetEnumerator() => new IndexEnumerable(_buffer);
        IEnumerator<Index> IEnumerable<Index>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
