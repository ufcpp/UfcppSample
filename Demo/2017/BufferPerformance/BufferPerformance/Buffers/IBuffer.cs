using System;

namespace BufferPerformance.Buffers
{
    interface IBuffer<TSpan> : IDisposable
        where TSpan : IByteSpan
    {
        void Reserve(int length);
        void Skip(int length);
        unsafe void Append(byte* data, int length);
        TSpan WrittenSpan { get; }
        TSpan FreeSpan { get; }
    }
}
