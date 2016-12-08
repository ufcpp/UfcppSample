using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Utf8;

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

        public static void Pack(DefinitionData.Sample d, Func<int, int> getIndex, ref Sample g)
        {
            g.Id = d.Id;
            g.A = d.A;
            g.B = d.B;
            g.C = d.C;
            g.DIndex = getIndex(d.D.Length);
        }

        public static IEnumerable<ReadOnlySpan<byte>> GetBinaries(DefinitionData.Sample d)
        {
            yield return d.D.Bytes;
        }
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

        public static void Pack(DefinitionData.Sample d, Func<int, int> getIndex, ref Sample_A_B_C g)
        {
            g.A = d.A;
            g.B = d.B;
            g.C = d.C;
        }

        public static IEnumerable<ReadOnlySpan<byte>> GetBinaries(DefinitionData.Sample d)
        {
            yield break;
        }
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

        public static void Pack(DefinitionData.Sample d, Func<int, int> getIndex, ref Sample_Id_B_D g)
        {
            g.Id = d.Id;
            g.B = d.B;
            g.DIndex = getIndex(d.D.Length);
        }

        public static IEnumerable<ReadOnlySpan<byte>> GetBinaries(DefinitionData.Sample d)
        {
            yield return d.D.Bytes;
        }
    }
}
