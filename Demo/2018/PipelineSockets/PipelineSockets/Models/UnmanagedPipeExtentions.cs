using System.Buffers;
using System.Runtime.InteropServices;

namespace PipelineSockets
{
    static class UnmanagedPipeExtentions
    {
        public static unsafe void Write<T>(this IBufferWriter<byte> w, T x)
            where T : unmanaged
        {
            MemoryMarshal.Cast<byte, T>(w.GetSpan(sizeof(T)))[0] = x;
            w.Advance(sizeof(T));
        }

        public static unsafe bool Read<T>(ref this BufferReader<byte> r, out T x)
            where T : unmanaged
        {
            var temp = default(T);
            var buffer = MemoryMarshal.Cast<T, byte>(MemoryMarshal.CreateSpan(ref temp, 1));
            var span = r.Peek(buffer);
            if(span.Length < sizeof(T))
            {
                x = default;
                return false;
            }
            x = MemoryMarshal.Cast<byte, T>(span)[0];
            r.Advance(sizeof(T));
            return true;
        }
    }
}
