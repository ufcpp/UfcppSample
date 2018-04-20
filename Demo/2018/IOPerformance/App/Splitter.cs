using System;

namespace App
{
    readonly ref struct Splitter
    {
        readonly Span<byte> _data;
        readonly Span<int> _indexes;

        public Splitter(Span<byte> data, Span<int> indexBuffer, ReadOnlySpan<byte> delimiter)
        {
            _data = data;
            _indexes = indexBuffer;

            var index = 0;
            for (int i = 0; i < indexBuffer.Length; i++)
            {
                if (index != data.Length)
                {
                    var next = data.Slice(index + 1).IndexOf(delimiter);
                    index = next < 0 ? data.Length : index + 1 + next;
                }
                indexBuffer[i] = index;
            }
        }

        public Span<byte> this[int index]
        {
            get
            {
                if ((uint)index >= _indexes.Length) throw new IndexOutOfRangeException();
                var s = index == 0 ? 0 : _indexes[index - 1] + 1;
                var e = _indexes[index];
                if (e <= s) return default;
                return _data.Slice(s, e - s);
            }
        }
    }
}
