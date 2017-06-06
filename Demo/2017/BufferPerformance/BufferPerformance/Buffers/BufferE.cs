using System;
using System.Runtime.InteropServices;

namespace BufferPerformance.Buffers
{
    unsafe struct BufferE : IBuffer<PointerSpan>
    {
        private byte* _p;
        private int _length;
        private int _writeLength;

        public BufferE(int capacity)
        {
            _length = capacity;
            _p = (byte*)Marshal.AllocCoTaskMem(capacity);
            _writeLength = 0;
        }

        public void Reserve(int length)
        {
            if (_writeLength + length >= _length)
            {
                var newLength = Math.Max(_length * 2, length);
                var p = (byte*)Marshal.AllocCoTaskMem(newLength);

                Buffer.MemoryCopy(_p, p, newLength, _writeLength);
                Marshal.FreeCoTaskMem((IntPtr)_p);

                _p = p;
                _length = newLength;
            }
        }
        public void Skip(int length) => _writeLength += length;

        public unsafe void Append(byte* data, int length)
        {
            Reserve(length);
            Buffer.MemoryCopy(data, _p + _writeLength, _length - _writeLength, length);
            Skip(length);
        }

        public void Dispose()
        {
            Console.WriteLine("b");
            Marshal.FreeCoTaskMem((IntPtr)_p);
            _p = default(byte*);
            Console.WriteLine("c");
        }

        public PointerSpan WrittenSpan => new PointerSpan(_p, _writeLength);
        public PointerSpan FreeSpan => new PointerSpan(_p + _writeLength, _length - _writeLength);
    }
}
