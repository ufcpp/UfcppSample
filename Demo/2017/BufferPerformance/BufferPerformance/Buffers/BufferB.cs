using System;
using System.Buffers;

namespace BufferPerformance.Buffers
{
    unsafe struct BufferB : IBuffer<ArraySpan>
    {
        private static ArrayPool<byte> _pool = ArrayPool<byte>.Shared;

        private byte[] _buffer;
        private int _writeLength;

        public BufferB(int capacity)
        {
            _buffer = _pool.Rent(capacity);
            _writeLength = 0;
        }

        public void Reserve(int length)
        {
            if (_writeLength + length >= _buffer.Length)
            {
                var capacity = Math.Max(_buffer.Length * 2, length);
                var buffer = _pool.Rent(capacity);
                Array.Copy(_buffer, buffer, _writeLength);
                _pool.Return(_buffer);
                _buffer = buffer;
            }
        }
        public void Skip(int length) => _writeLength += length;

        public unsafe void Append(byte* data, int length)
        {
            Reserve(length);
            fixed (byte* p = _buffer)
                System.Buffer.MemoryCopy(data, p + _writeLength, _buffer.Length - _writeLength, length);
            Skip(length);
        }

        public void Dispose()
        {
            _pool.Return(_buffer);
            _buffer = null;
        }

        public ArraySpan WrittenSpan => new ArraySpan(_buffer, 0, _writeLength);
        public ArraySpan FreeSpan => new ArraySpan(_buffer, _writeLength, _buffer.Length - _writeLength);
    }
}
