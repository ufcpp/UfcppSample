using System.Text.Utf8;

namespace FixedPacker.Samples.DefinitionData
{
    public class Sample
    {
        public int Id { get; }

        public byte A { get; }

        public long B { get; }

        public short C { get; }

        public Utf8String D { get; }

        public Sample(int id, byte a, long b, short c, Utf8String d)
        {
            Id = id;
            A = a;
            B = b;
            C = c;
            D = d;
        }
    }
}
