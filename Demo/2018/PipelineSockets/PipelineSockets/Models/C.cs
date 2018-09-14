using System.Buffers;

namespace PipelineSockets.Models
{
    struct C
    {
        Vector U;
        Vector V;

        public void WriteTo(IBufferWriter<byte> w)
        {
            w.Write(U);
            w.Write(V);
        }

        public bool ReadFrom(ref BufferReader<byte> r)
            =>
                r.Read(out U) &&
                r.Read(out V);
    }

    static partial class Extensions
    {
        public static void Write(this IBufferWriter<byte> w, C x) => x.WriteTo(w);

        public static unsafe bool Read(ref this BufferReader<byte> r, out C x)
        {
            x = default;
            return x.ReadFrom(ref r);
        }
    }
}
