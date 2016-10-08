using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApplication1.StringUtilities
{
    struct Utf8String : IEnumerable<CodePoint>
    {
        private readonly byte[] _buffer;

        public Utf8String(byte[] encodedBytes) : this()
        {
            _buffer = encodedBytes;
        }

        public Utf8StringEnumerator GetEnumerator() => new Utf8StringEnumerator(_buffer);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator<CodePoint> IEnumerable<CodePoint>.GetEnumerator() => GetEnumerator();
    }

    struct Utf8StringEnumerator : IEnumerator<CodePoint>
    {
        private readonly byte[] _buffer;
        private int _index;

        public Utf8StringEnumerator(byte[] buffer)
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

            if (x >= 0b1111_0000)
            {
                // 4バイト文字
                var code = x & 0b0111;
                if (!TryNext(out x)) return false;
                code = (code << 6) | x;
                if (!TryNext(out x)) return false;
                code = (code << 6) | x;
                if (!TryNext(out x)) return false;
                code = (code << 6) | x;

                Current = new CodePoint(code);
                return true;
            }
            if (x >= 0b1110_0000)
            {
                // 3バイト文字
                var code = x & 0b1111;
                if (!TryNext(out x)) return false;
                code = (code << 6) | x;
                if (!TryNext(out x)) return false;
                code = (code << 6) | x;

                Current = new CodePoint(code);
                return true;
            }
            if (x >= 0b1100_0000)
            {
                // 2バイト文字
                var code = x & 0b1_1111;
                if (!TryNext(out x)) return false;
                code = (code << 6) | x;

                Current = new CodePoint(code);
                return true;
            }

            // ASCII 文字
            Current = new CodePoint(x);
            return true;
        }

        private bool TryNext(out uint code)
        {
            code = 0;
            if (_index >= _buffer.Length) return false;

            var c = _buffer[_index++];
            if ((c & 0b1100_0000) != 0b1000_0000) return false;

            code = (uint)(c & 0b0011_1111);
            return true;
        }

        object IEnumerator.Current => Current;
        void IDisposable.Dispose() { }
        public void Reset() => _index = 0;
    }
}
