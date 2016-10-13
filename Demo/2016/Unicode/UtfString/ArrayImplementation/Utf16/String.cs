using System.Collections;
using System.Collections.Generic;

namespace UtfString.ArrayImplementation.Utf16
{
    public struct String : IEnumerable<CodePoint>, IString<Index, StringEnumerator, IndexEnumerable, IndexEnumerable>
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
    }
}
