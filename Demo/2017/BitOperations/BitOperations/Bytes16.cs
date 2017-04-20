using System;

namespace BitOperations
{
    /// <summary>
    /// A 16-byte-sized struct.
    /// I assume little-endianness.
    /// </summary>
    public struct Bytes16
    {
        public ulong LowWord;
        public ulong HighWord;
    }

    public struct Bytes16BitOperator : SBitOperator<Bytes16>
    {
        public int Size => 128;
        public bool GetBit(ref Bytes16 x, int index)
        {
            if (index < 0 || index >= 128) throw new IndexOutOfRangeException();

            if (index < 64) return (x.LowWord & (1UL << index)) != 0;
            else return (x.HighWord & (1UL << (index - 64))) != 0;
        }
        public void SetBit(ref Bytes16 x, int index, bool value)
        {
            if (index < 64)
            {
                if (value) x.LowWord |= (1UL << index);
                else x.LowWord &= ~(1UL << index);
            }
            else
            {
                index -= 64;
                if (value) x.HighWord |= (1UL << index);
                else x.HighWord &= ~(1UL << index);
            }
        }
        public Bytes16 RightShift(Bytes16 x)
        {
            var lowestHigh = (x.HighWord & 1);
            x.HighWord >>= 1;
            x.LowWord >>= 1;
            if (lowestHigh == 1) x.LowWord |= 0x8000_0000_0000_0000;
            return x;
        }
    }
}
