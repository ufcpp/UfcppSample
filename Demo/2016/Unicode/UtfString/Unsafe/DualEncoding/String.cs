using System.Collections;
using System.Collections.Generic;

namespace UtfString.Unsafe.DualEncoding
{
    public struct String : IEnumerable<CodePoint>, IString<Index, StringEnumerator, IndexEnumerable, IndexEnumerable>
    {
        private readonly ArrayAccessor _buffer;

        public String(bool isWideChar, byte[] encodedBytes)
        {
            _buffer = new ArrayAccessor(isWideChar, encodedBytes);
        }

        public StringEnumerator GetEnumerator() => new StringEnumerator(_buffer);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator<CodePoint> IEnumerable<CodePoint>.GetEnumerator() => GetEnumerator();

        public IndexEnumerable Indexes => new IndexEnumerable(_buffer);
        public CodePoint this[Index index] => Decoder.Decode(_buffer, index);

        public int Length => Decoder.GetCount(_buffer);
    }
}
