namespace UtfString.Slices
{
    /// <summary>
    /// https://github.com/dotnet/corefxlab/blob/master/src/System.Slices/System/Span.cs もどき。
    /// 簡単化のために byte[] 限定に変更。
    /// </summary>
    public struct ReadOnlySpan
    {
        private byte[] _array;
        private int _index;
        private int _length;

        public ReadOnlySpan(byte[] array) : this(array, 0, array.Length) { }
        public ReadOnlySpan(byte[] array, int index) : this(array, index, array.Length - index) { }
        public ReadOnlySpan(byte[] array, int index, int length)
        {
            _array = array;
            _index = index;
            _length = length;
        }

        public static implicit operator ReadOnlySpan(byte[] array) => new ReadOnlySpan(array);

        public byte this[int index] => _array[_index + index];
        public int Length => _length;

        public ReadOnlySpan Slice(int index, int length) => new ReadOnlySpan(_array, _index + index, length);

        public struct Enumerator
        {
            private byte[] _array;
            private int _index;
            private int _end;

            public Enumerator(ReadOnlySpan span)
            {
                _array = span._array;
                _index = span._index - 1;
                _end = span._index + span._length;
            }

            public bool MoveNext()
            {
                _index++;
                return _index < _end;
            }

            public byte Current => _array[_index];
        }

        public Enumerator GetEnumerator() => new Enumerator(this);
    }
}
