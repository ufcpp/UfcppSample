using System.Collections;
using System.Collections.Generic;

namespace UtfString.Utf8
{
    public struct String : IEnumerable<CodePoint>
    {
        private readonly byte[] _buffer;

        public String(byte[] encodedBytes) : this()
        {
            _buffer = encodedBytes;
        }

        public StringEnumerator GetEnumerator() => new StringEnumerator(_buffer);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator<CodePoint> IEnumerable<CodePoint>.GetEnumerator() => GetEnumerator();

        public IndexEnumerable Indexes => new IndexEnumerable(_buffer);
        public CodePoint this[Index index] => Decoder.Decode(_buffer, index);

        public int Length => Decoder.GetByteCount(_buffer);
    }
}
