namespace MetricSpace
{
    public struct IntArithmetic : IArithmetic<int>
    {
        public int MinValue => int.MinValue;
        public int MaxValue => int.MaxValue;
        public int Zero => 0;
        public int NegativeInfinity => int.MinValue;
        public int PositiveInfinity => int.MaxValue;
        public int Add(int a, int b) => a + b;
        public int Subtract(int a, int b) => a - b;
        public int Multiply(int a, int b) => a * b;
    }

    public struct ShortArithmetic : IArithmetic<short>
    {
        public short MinValue => short.MinValue;
        public short MaxValue => short.MaxValue;
        public short Zero => 0;
        public short NegativeInfinity => short.MinValue;
        public short PositiveInfinity => short.MaxValue;
        public short Add(short a, short b) => (short)(a + b);
        public short Subtract(short a, short b) => (short)(a - b);
        public short Multiply(short a, short b) => (short)(a * b);
    }

    public struct LongArithmetic : IArithmetic<long>
    {
        public long MinValue => long.MinValue;
        public long MaxValue => long.MaxValue;
        public long Zero => 0;
        public long NegativeInfinity => long.MinValue;
        public long PositiveInfinity => long.MaxValue;
        public long Add(long a, long b) => a + b;
        public long Subtract(long a, long b) => a - b;
        public long Multiply(long a, long b) => a * b;
    }
}
