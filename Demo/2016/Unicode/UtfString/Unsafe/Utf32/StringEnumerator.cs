using System;
using System.Collections;
using System.Collections.Generic;

namespace UtfString.Unsafe.Utf32
{
    public struct StringEnumerator : IEnumerator<CodePoint>
    {
        private readonly ArrayAccessor _buffer;
        private int _index;
        private bool _init;

        public StringEnumerator(ArrayAccessor buffer)
        {
            _buffer = buffer;
            _index = 0;
            _init = false;
        }

        public CodePoint Current => new CodePoint(_buffer[_index]);

        public bool MoveNext()
        {
            if (!_init) _init = true;
            else _index++;
            return _index < _buffer.Length;
        }

        object IEnumerator.Current => Current;
        void IDisposable.Dispose() { }
        void IEnumerator.Reset() { throw new NotSupportedException(); }
    }
}
