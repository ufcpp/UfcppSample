using System.Buffers;

namespace PipelineSockets.Models
{
    struct B
    {
        int Id;
        IntVector P;
        IntVector Q;

        public void WriteTo(IBufferWriter<byte> w)
        {
            w.Write(Id);
            w.Write(P);
            w.Write(Q);
        }

        public bool ReadFrom(ref BufferReader<byte> r)
            =>
                r.Read(out Id) &&
                r.Read(out P) &&
                r.Read(out Q);
    }

    static partial class Extensions
    {
        public static void Write(this IBufferWriter<byte> w, B x) => x.WriteTo(w);

        public static unsafe bool Read(ref this BufferReader<byte> r, out B x)
        {
            x = default;
            return x.ReadFrom(ref r);
        }
    }
}
