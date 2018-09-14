using System.Buffers;

namespace PipelineSockets.Models
{
    struct IntVector
    {
        public int X;
        public int Y;
        public int Z;

        public void WriteTo(IBufferWriter<byte> w)
        {
            w.Write(X);
            w.Write(Y);
            w.Write(Z);
        }

        public bool ReadFrom(ref BufferReader<byte> r)
            =>
                r.Read(out X) &&
                r.Read(out Y) &&
                r.Read(out Z);
    }

    static partial class Extensions
    {
        public static void Write(this IBufferWriter<byte> w, IntVector x) => x.WriteTo(w);

        public static unsafe bool Read(ref this BufferReader<byte> r, out IntVector x)
        {
            x = default;
            return x.ReadFrom(ref r);
        }
    }
}
