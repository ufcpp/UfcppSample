using System;
using System.Collections;
using System.Collections.Generic;

namespace UtfString.Utf8
{
    public struct StringEnumerator : IEnumerator<CodePoint>
    {
        private readonly byte[] _buffer;
        private int _index;
        private CodePoint _current;

        public StringEnumerator(byte[] buffer)
        {
            _buffer = buffer;
            _index = 0;
            _current = default(CodePoint);
        }

        public CodePoint Current => _current;
        public bool MoveNext() => Decoder.TryDecode(_buffer, ref _index, out _current);

        object IEnumerator.Current => Current;
        void IDisposable.Dispose() { }
        public void Reset() { throw new NotSupportedException(); }
    }
}
