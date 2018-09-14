using System.Buffers;

namespace PipelineSockets.Models
{
    struct A
    {
        public int Id;
        public string Name;

        public void WriteTo(IBufferWriter<byte> w)
        {
            w.Write(Id);
            w.Write(Name);
        }

        public bool ReadFrom(ref BufferReader<byte> r)
            =>
                r.Read(out Id) &&
                r.Read(out Name);
    }

    static partial class Extensions
    {
        public static void Write(this IBufferWriter<byte> w, A x) => x.WriteTo(w);

        public static unsafe bool Read(ref this BufferReader<byte> r, out A x)
        {
            x = default;
            return x.ReadFrom(ref r);
        }
    }
}
