using System;

namespace BitOperations
{
    // T4 とかで生成する方がいいかも。
    // ほぼコピペで byte, ushort, uint, ulong 用の SBitOperator を用意。

    public struct ByteBitOperator : SBitOperator<byte>
    {
        public int Size => 8;
        public bool GetBit(ref byte x, int index)
        {
            if (index < 0 || index >= Size) throw new IndexOutOfRangeException();
            return (x & (1 << index)) != 0;
        }
        public void SetBit(ref byte x, int index, bool value)
        {
            if (index < 0 || index >= Size) throw new IndexOutOfRangeException();
            if (value) x |= (byte)(1 << index);
            else x &= (byte)~(1 << index);
        }
        public byte RightShift(byte x) => (byte)(x >> 1);
    }

    public struct ShortBitOperator : SBitOperator<ushort>
    {
        public int Size => 16;
        public bool GetBit(ref ushort x, int index)
        {
            if (index < 0 || index >= Size) throw new IndexOutOfRangeException();
            return (x & (1 << index)) != 0;
        }
        public void SetBit(ref ushort x, int index, bool value)
        {
            if (index < 0 || index >= Size) throw new IndexOutOfRangeException();
            if (value) x |= (ushort)(1 << index);
            else x &= (ushort)~(1 << index);
        }
        public ushort RightShift(ushort x) => (ushort)(x >> 1);
    }

    public struct IntBitOperator : SBitOperator<uint>
    {
        public int Size => 32;
        public bool GetBit(ref uint x, int index)
        {
            if (index < 0 || index >= Size) throw new IndexOutOfRangeException();
            return (x & (1 << index)) != 0;
        }
        public void SetBit(ref uint x, int index, bool value)
        {
            if (index < 0 || index >= Size) throw new IndexOutOfRangeException();
            if (value) x |= (uint)(1 << index);
            else x &= (uint)~(1 << index);
        }
        public uint RightShift(uint x) => (x >> 1);
    }

    public struct LongBitOperator : SBitOperator<ulong>
    {
        public int Size => 64;
        public bool GetBit(ref ulong x, int index)
        {
            if (index < 0 || index >= Size) throw new IndexOutOfRangeException();
            return (x & (1UL << index)) != 0;
        }
        public void SetBit(ref ulong x, int index, bool value)
        {
            if (index < 0 || index >= Size) throw new IndexOutOfRangeException();
            if (value) x |= 1UL << index;
            else x &= ~(1UL << index);
        }
        public ulong RightShift(ulong x) => (x >> 1);
    }
}
