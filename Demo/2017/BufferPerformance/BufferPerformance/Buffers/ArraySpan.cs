using System;

namespace BufferPerformance.Buffers
{
    struct ArraySpan : IByteSpan
    {
        ArraySegment<byte> _segment;
        public ArraySpan(byte[] array, int offset, int count) => _segment = new ArraySegment<byte>(array, offset, count);
        public ArraySpan(ArraySegment<byte> segment) => _segment = segment;
        public ref byte this[int index] => ref _segment.Array[_segment.Offset + index];
        public int Length => _segment.Count;
    }
}
