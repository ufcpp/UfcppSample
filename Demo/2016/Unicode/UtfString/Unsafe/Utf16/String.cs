using System.Collections;
using System.Collections.Generic;

namespace UtfString.Unsafe.Utf16
{
    public struct String : IEnumerable<CodePoint>, IString<Index, StringEnumerator, IndexEnumerable, IndexEnumerable>
    {
        private readonly ArrayAccessor _buffer;

        public String(byte[] encodedBytes)
        {
            _buffer = new ArrayAccessor(encodedBytes);
        }

        public StringEnumerator GetEnumerator() => new StringEnumerator(_buffer);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator<CodePoint> IEnumerable<CodePoint>.GetEnumerator() => GetEnumerator();

        public IndexEnumerable Indexes => new IndexEnumerable(_buffer);
        public CodePoint this[Index index] => Decoder.Decode(_buffer, index);

        public int Length => Decoder.GetCount(_buffer);
    }
}
