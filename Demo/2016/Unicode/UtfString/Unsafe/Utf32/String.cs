using System.Collections;
using System.Collections.Generic;

namespace UtfString.Unsafe.Utf32
{
    public struct String : IEnumerable<CodePoint>, IString<Index, StringEnumerator, IndexEnumerable, IndexEnumerable>
    {
        private readonly UIntAccessor _buffer;

        public String(byte[] encodedBytes)
        {
            _buffer = new UIntAccessor(encodedBytes);
        }

        public StringEnumerator GetEnumerator() => new StringEnumerator(_buffer);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator<CodePoint> IEnumerable<CodePoint>.GetEnumerator() => GetEnumerator();

        public IndexEnumerable Indexes => new IndexEnumerable(_buffer);
        public CodePoint this[Index index] => new CodePoint(_buffer[index.index]);

        public int Length => _buffer.Length;
    }
}
