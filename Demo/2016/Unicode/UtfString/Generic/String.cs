using System;
using System.Collections;
using System.Collections.Generic;

namespace UtfString.Generic
{
    public struct String<TChar, TArrayAccessor, TDecoder> : IEnumerable<CodePoint>, IString<Index, StringEnumerator<TChar, TArrayAccessor, TDecoder>, IndexEnumerable<TChar, TArrayAccessor, TDecoder>, IndexEnumerable<TChar, TArrayAccessor, TDecoder>>
        where TChar : struct
        where TArrayAccessor : struct, IArrayAccessor<TChar>
        where TDecoder : struct, IDecoder<TChar, TArrayAccessor>
    {
        private readonly TArrayAccessor _buffer;

        public String(TArrayAccessor array)
        {
            _buffer = array;
        }

        public StringEnumerator<TChar, TArrayAccessor, TDecoder> GetEnumerator() => new StringEnumerator<TChar, TArrayAccessor, TDecoder>(_buffer);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator<CodePoint> IEnumerable<CodePoint>.GetEnumerator() => GetEnumerator();

        public IndexEnumerable<TChar, TArrayAccessor, TDecoder> Indexes => new IndexEnumerable<TChar, TArrayAccessor, TDecoder>(_buffer);
        public CodePoint this[Index index] => default(TDecoder).Decode(_buffer, index);

        public int Length => default(TDecoder).GetLength(_buffer);
    }

    public struct Index
    {
        internal int index;
        internal byte count;
    }

    public struct StringEnumerator<TChar, TArrayAccessor, TDecoder> : IEnumerator<CodePoint>
        where TChar : struct
        where TArrayAccessor : struct, IArrayAccessor<TChar>
        where TDecoder : struct, IDecoder<TChar, TArrayAccessor>
    {
        private readonly TArrayAccessor _buffer;
        private Index _index;

        public StringEnumerator(TArrayAccessor buffer)
        {
            _buffer = buffer;
            _index = default(Index);
            Current = default(CodePoint);
        }

        public CodePoint Current { get; private set; }

        public bool MoveNext()
        {
            _index.index += _index.count;
            var next = default(TDecoder).TryDecode(_buffer, _index.index);
            if (next.count == Constants.InvalidCount) return false;
            _index.count = next.count;
            Current = next.cp;
            return true;
        }

        object IEnumerator.Current => Current;
        void IDisposable.Dispose() { }
        void IEnumerator.Reset() { throw new NotSupportedException(); }
    }

    public struct IndexEnumerable<TChar, TArrayAccessor, TDecoder> : IEnumerator<Index>, IEnumerable<Index>, IIndexEnumerable<Index, IndexEnumerable<TChar, TArrayAccessor, TDecoder>>
        where TChar : struct
        where TArrayAccessor : struct, IArrayAccessor<TChar>
        where TDecoder : struct, IDecoder<TChar, TArrayAccessor>
    {
        private readonly TArrayAccessor _buffer;
        private Index _i;

        public IndexEnumerable(TArrayAccessor buffer)
        {
            _buffer = buffer;
            _i = default(Index);
        }

        public Index Current => _i;
        public bool MoveNext()
        {
            _i.index += _i.count;
            _i.count = default(TDecoder).TyrGetCount(_buffer, _i.index);
            return _i.count != Constants.InvalidCount;
        }

        object IEnumerator.Current => Current;
        void IDisposable.Dispose() { }
        void IEnumerator.Reset() { throw new NotSupportedException(); }

        public IndexEnumerable<TChar, TArrayAccessor, TDecoder> GetEnumerator() => new IndexEnumerable<TChar, TArrayAccessor, TDecoder>(_buffer);
        IEnumerator<Index> IEnumerable<Index>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
