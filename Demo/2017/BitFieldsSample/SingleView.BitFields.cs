using BitFields;

namespace BitFieldsSample
{
    partial struct SingleView
    {
        public uint Value;

        private const int FractionShift = 0;
        private const uint FractionMask = unchecked((uint)((1U << 23) - (1U << 0)));
        public Bit23 Fraction
        {
            get => (Bit23)((Value & FractionMask) >> FractionShift);
            set => Value = unchecked((uint)((Value & ~FractionMask) | ((((uint)value) << FractionShift) & FractionMask)));
        }
        private const int ExponentShift = 23;
        private const uint ExponentMask = unchecked((uint)((1U << 31) - (1U << 23)));
        public Bit8 Exponent
        {
            get => (Bit8)((Value & ExponentMask) >> ExponentShift);
            set => Value = unchecked((uint)((Value & ~ExponentMask) | ((((uint)value) << ExponentShift) & ExponentMask)));
        }
        private const int SignShift = 31;
        private const uint SignMask = unchecked((uint)((1U << 32) - (1U << 31)));
        public Bit1 Sign
        {
            get => (Bit1)((Value & SignMask) >> SignShift);
            set => Value = unchecked((uint)((Value & ~SignMask) | ((((uint)value) << SignShift) & SignMask)));
        }
    }
}
