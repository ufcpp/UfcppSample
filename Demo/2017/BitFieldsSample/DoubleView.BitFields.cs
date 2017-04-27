using BitFields;

namespace BitFieldsSample
{
    partial struct DoubleView
    {
        public ulong Value;

        private const int FractionShift = 0;
        private const ulong FractionMask = unchecked((ulong)((1UL << 52) - (1UL << 0)));
        public Bit52 Fraction
        {
            get => (Bit52)((Value & FractionMask) >> FractionShift);
            set => Value = unchecked((ulong)((Value & ~FractionMask) | ((((ulong)value) << FractionShift) & FractionMask)));
        }
        private const int ExponentShift = 52;
        private const ulong ExponentMask = unchecked((ulong)((1UL << 63) - (1UL << 52)));
        public Bit11 Exponent
        {
            get => (Bit11)((Value & ExponentMask) >> ExponentShift);
            set => Value = unchecked((ulong)((Value & ~ExponentMask) | ((((ulong)value) << ExponentShift) & ExponentMask)));
        }
        private const int SignShift = 63;
        private const ulong SignMask = unchecked((ulong)((1UL << 64) - (1UL << 63)));
        public Bit1 Sign
        {
            get => (Bit1)((Value & SignMask) >> SignShift);
            set => Value = unchecked((ulong)((Value & ~SignMask) | ((((ulong)value) << SignShift) & SignMask)));
        }
    }
}
