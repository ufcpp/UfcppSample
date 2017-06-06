using System;

namespace BufferPerformance.Buffers
{
    unsafe struct BufferA : IBuffer<ArraySpan>
    {
        private byte[] _buffer;
        private int _writeLength;

        public BufferA(int capacity)
        {
            _buffer = new byte[capacity];
            _writeLength = 0;
        }

        public void Reserve(int length)
        {
            if (_writeLength + length >= _buffer.Length)
            {
                var capacity = Math.Max(_buffer.Length * 2, length);
                var buffer = new byte[capacity];
                Array.Copy(_buffer, buffer, _writeLength);
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
            _buffer = null;
        }

        public ArraySpan WrittenSpan => new ArraySpan(_buffer, 0, _writeLength);
        public ArraySpan FreeSpan => new ArraySpan(_buffer, _writeLength, _buffer.Length - _writeLength);
    }

}
