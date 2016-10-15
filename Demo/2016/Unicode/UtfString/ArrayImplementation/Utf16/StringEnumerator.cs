using System;
using System.Collections;
using System.Collections.Generic;

namespace UtfString.ArrayImplementation.Utf16
{
    public struct StringEnumerator : IEnumerator<CodePoint>
    {
        private readonly ushort[] _buffer;
        private int _index;

        public StringEnumerator(ushort[] buffer)
        {
            _buffer = buffer;
            _index = 0;
            Current = default(CodePoint);
        }

        public CodePoint Current { get; private set; }

        public bool MoveNext()
        {
            if (_index >= _buffer.Length) return false;
            uint x = _buffer[_index++];

            if ((x & 0b1111_1100_0000_0000) == 0b1101_1000_0000_0000)
            {
                var code = (x & 0b0011_1111_1111) + 0b0100_0000;
                if (_index >= _buffer.Length) return false;
                x = _buffer[_index++];
                if ((x & 0b1111_1100_0000_0000) != 0b1101_1100_0000_0000) return false;
                code = (code << 10) | (x & 0b0011_1111_1111);

                Current = new CodePoint(code);
                return true;
            }
            else
            {
                Current = new CodePoint(x);
                return true;
            }
        }

        object IEnumerator.Current => Current;
        void IDisposable.Dispose() { }
        void IEnumerator.Reset() { throw new NotSupportedException(); }
    }
}
