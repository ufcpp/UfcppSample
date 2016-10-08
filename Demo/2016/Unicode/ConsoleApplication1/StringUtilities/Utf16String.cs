using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApplication1.StringUtilities
{
    struct Utf16String : IEnumerable<CodePoint>
    {
        private readonly ushort[] _buffer;

        public Utf16String(byte[] encodedBytes) : this()
        {
            // 通常の実装だと BlockCopy が必要だけど、little endian に限定して unsafe コードを使えばコピー不要
            if ((encodedBytes.Length % 2) != 0) throw new ArgumentException();
            _buffer = new ushort[encodedBytes.Length / 2];
            Buffer.BlockCopy(encodedBytes, 0, _buffer, 0, encodedBytes.Length);
        }

        public Utf16StringEnumerator GetEnumerator() => new Utf16StringEnumerator(_buffer);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator<CodePoint> IEnumerable<CodePoint>.GetEnumerator() => GetEnumerator();
    }

    struct Utf16StringEnumerator : IEnumerator<CodePoint>
    {
        private readonly ushort[] _buffer;
        private int _index;

        public Utf16StringEnumerator(ushort[] buffer)
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
                // サロゲート ペアの処理
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
                // 利用頻度が高い文字はほぼこちら側に来る
                // バッファー内の値を素通し。
                Current = new CodePoint(x);
                return true;
            }
        }

        object IEnumerator.Current => Current;
        void IDisposable.Dispose() { }
        public void Reset() => _index = 0;
    }
}
