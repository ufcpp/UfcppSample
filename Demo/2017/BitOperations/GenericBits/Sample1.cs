namespace GenericBits
{
    struct Sample1
    {
        public byte A;
        public byte B;
        public ushort C;
        public uint D;
        public Sample1(byte a, byte b, ushort c, uint d) => (A, B, C, D) = (a, b, c, d);
        public override string ToString() => (A, B, C, D).ToString();
    }
}