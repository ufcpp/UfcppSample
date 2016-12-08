using System.Runtime.InteropServices;

namespace FixedPacker.Samples.GeneratedData
{
    [StructLayout(LayoutKind.Explicit, Pack = 8)]
    public struct Sample
    {
        [FieldOffset(0)]
        public int Id;

        [FieldOffset(4)]
        public byte A;

        [FieldOffset(8)]
        public long B;

        [FieldOffset(6)]
        public short C;

        [FieldOffset(16)]
        public int DIndex;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 8)]
    public struct Sample_A_B_C
    {
        [FieldOffset(0)]
        public byte A;

        [FieldOffset(8)]
        public long B;

        [FieldOffset(2)]
        public short C;
    }
    [StructLayout(LayoutKind.Explicit, Pack = 8)]
    public struct Sample_Id_B_D
    {
        [FieldOffset(0)]
        public int Id;

        [FieldOffset(8)]
        public long B;

        [FieldOffset(4)]
        public int DIndex;
    }
}
