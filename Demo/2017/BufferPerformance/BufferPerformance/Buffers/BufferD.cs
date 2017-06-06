using System;
using System.Buffers;
using System.Runtime.InteropServices;

namespace BufferPerformance.Buffers
{
    unsafe struct BufferD : IBuffer<PointerSpan>
    {
        private static ArrayPool<byte> _pool = ArrayPool<byte>.Shared;

        private GCHandle _p;
        private int _length;
        private int _writeLength;

        public BufferD(int capacity)
        {
            Allocate(capacity, out _p, out _length);
            _writeLength = 0;
        }

        private static void Allocate(int capacity, out GCHandle p, out int length)
        {
            var buffer = _pool.Rent(capacity);
            p = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            length = buffer.Length;
        }

        public void Reserve(int length)
        {
            if (_writeLength + length >= _length)
            {
                var buffer = (byte[])_p.Target;
                _p.Free();

                var capacity = Math.Max(_length * 2, length);
                Allocate(capacity, out var p, out var newLength);

                Array.Copy(buffer, (byte[])p.Target, _writeLength);

                ArrayPool<byte>.Shared.Return(buffer);

                _p = p;
                _length = newLength;
            }
        }
        public void Skip(int length) => _writeLength += length;

        public unsafe void Append(byte* data, int length)
        {
            Reserve(length);
            Buffer.MemoryCopy(data, (byte*)_p.AddrOfPinnedObject() + _writeLength, _length - _writeLength, length);
            Skip(length);
        }

        public void Dispose()
        {
            var buffer = (byte[])_p.Target;
            _pool.Return(buffer);
            _p.Free();
            _p = default(GCHandle);
        }

        public PointerSpan WrittenSpan => new PointerSpan((byte*)_p.AddrOfPinnedObject(), _writeLength);
        public PointerSpan FreeSpan => new PointerSpan((byte*)_p.AddrOfPinnedObject() + _writeLength, _length - _writeLength);
    }
}
