using System;
using System.Buffers;
using System.Runtime.InteropServices;

namespace PipelineSockets
{
    static class StringPipeExtentions
    {
        public static void Write(this IBufferWriter<byte> w, string x)
        {
            var span = w.GetSpan(x.Length * sizeof(char) + sizeof(int));
            w.Write(x.Length);
            var body = span.Slice(sizeof(int));
            MemoryMarshal.Cast<char, byte>(x.AsSpan()).CopyTo(body);
        }

        public static unsafe bool Read(ref this BufferReader<byte> r, out string x)
        {
            r.Read(out int len);

            var s = new string('\0', len);
            fixed (char* p = s)
            {
                var span = new Span<byte>((byte*)p, len * 2);
                var readCount = r.CoptyTo(span);
                if (readCount < len * 2)
                {
                    x = default;
                    return false;
                }
            }

            x = s;
            return true;
        }
    }
}
