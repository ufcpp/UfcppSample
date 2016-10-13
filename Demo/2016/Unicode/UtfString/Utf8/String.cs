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
    }
}
