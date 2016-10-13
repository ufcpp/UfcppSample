using System;
using System.Collections;
using System.Collections.Generic;

namespace UtfString.Utf16
{
    public struct String : IEnumerable<CodePoint>
    {
        private readonly ushort[] _buffer;

        public String(byte[] encodedBytes) : this()
        {
            // 通常の実装だと BlockCopy が必要だけど、little endian に限定して unsafe コードを使えばコピー不要
            if ((encodedBytes.Length % 2) != 0) throw new ArgumentException();
            _buffer = new ushort[encodedBytes.Length / 2];
            Buffer.BlockCopy(encodedBytes, 0, _buffer, 0, encodedBytes.Length);
        }

        public StringEnumerator GetEnumerator() => new StringEnumerator(_buffer);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator<CodePoint> IEnumerable<CodePoint>.GetEnumerator() => GetEnumerator();
    }
}
