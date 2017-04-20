namespace GenericBits
{
    struct Sample2
    {
        public short A;
        public int B;
        public byte C;
        public long D;
        public Sample2(short a, int b, byte c, long d) => (A, B, C, D) = (a, b, c, d);
        public override string ToString() => (A, B, C, D).ToString();
    }
}