using System;
using System.Collections;
using System.Collections.Generic;

namespace UtfString.Slices
{
    public struct Utf8String : IEnumerable<CodePoint>
    {
        private readonly ReadOnlySpan _buffer;

        public Utf8String(ReadOnlySpan buffer)
        {
            _buffer = buffer;
        }

        public Enumerator GetEnumerator() => new Enumerator(_buffer);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator<CodePoint> IEnumerable<CodePoint>.GetEnumerator() => GetEnumerator();

        public int Length => Decoder.GetByteCount(_buffer);

        public struct Enumerator : IEnumerator<CodePoint>
        {
            private readonly ReadOnlySpan _buffer;
            private int _index;
            private CodePoint _current;

            public Enumerator(ReadOnlySpan buffer)
            {
                _buffer = buffer;
                _index = 0;
                _current = default(CodePoint);
            }

            public CodePoint Current => _current;
            public bool MoveNext() => Decoder.TryDecode(_buffer, ref _index, out _current);

            object IEnumerator.Current => Current;
            void IDisposable.Dispose() { }
            void IEnumerator.Reset() { throw new NotSupportedException(); }
        }
    }
}
