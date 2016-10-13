using System.Collections;
using System.Collections.Generic;

namespace UtfString.Utf16
{
    public struct String : IEnumerable<CodePoint>
    {
        private readonly ushort[] _buffer;

        public String(ushort[] encodedUshorts)
        {
            _buffer = encodedUshorts;
        }

        public StringEnumerator GetEnumerator() => new StringEnumerator(_buffer);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator<CodePoint> IEnumerable<CodePoint>.GetEnumerator() => GetEnumerator();

        public IndexEnumerable Indexes => new IndexEnumerable(_buffer);
        public CodePoint this[Index index] => Decoder.Decode(_buffer, index);

        public int Length => Decoder.GetCount(_buffer);

        public String(byte[] encodedBytes) : this(Copy8To16(encodedBytes))
        {
        }

        private static ushort[] Copy8To16(byte[] encodedBytes)
        {
            // 通常の実装だと BlockCopy が必要だけど、little endian に限定して unsafe な手段を使えばコピー不要になるかも
            if ((encodedBytes.Length % 2) != 0) throw new System.ArgumentException();
            var output = new ushort[encodedBytes.Length / 2];
            System.Buffer.BlockCopy(encodedBytes, 0, output, 0, encodedBytes.Length);
            return output;
        }
    }
}
