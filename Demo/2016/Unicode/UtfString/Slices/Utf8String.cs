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

        // この index/length は byte 配列のインデックスなので注意。文字数ベースじゃない。
        public Utf8String Substring(int index) => new Utf8String(_buffer.Slice(index));
        public Utf8String Substring(int index, int length) => new Utf8String(_buffer.Slice(index, length));

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

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            foreach (var c in this)
            {
                var v = c.Value;
                if (v < 0x10000)
                {
                    sb.Append((char)v);
                }
                else
                {
                    var highSurrogate = ((v - 0x010000u) >> 10) | 0xD800;
                    var lowSurrogate = (v & 0x38) | 0xDC00;
                    sb.Append((char)highSurrogate);
                    sb.Append((char)lowSurrogate);
                }
            }
            return sb.ToString();
        }
    }
}
