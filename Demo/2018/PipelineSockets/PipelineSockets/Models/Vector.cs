using System.Buffers;

namespace PipelineSockets.Models
{
    struct Vector
    {
        public float X;
        public float Y;
        public float Z;

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
        public static void Write(this IBufferWriter<byte> w, Vector x) => x.WriteTo(w);

        public static unsafe bool Read(ref this BufferReader<byte> r, out Vector x)
        {
            x = default;
            return x.ReadFrom(ref r);
        }
    }
}
