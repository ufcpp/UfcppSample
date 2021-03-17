using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace RgiSequenceFinder
{
    /// <summary>
    /// 長すぎる ZWJ シーケンスとかタグとかはサポートしなくていいと割り切って固定長 byte 列を構造体で作成。
    /// とりあえず8バイトで作成。
    /// </summary>
    /// <remarks>
    /// 参考までに、Unicode 13.0 の RGI 時点で、
    /// Emoji tag sequence のタグは6文字(しかない)
    /// ZWJ sequence 中の ZWJ の個数は
    /// </remarks>
    public struct Byte8 : IEquatable<Byte8>
    {
        public byte V0;
        public byte V1;
        public byte V2;
        public byte V3;
        public byte V4;
        public byte V5;
        public byte V6;
        public byte V7;

        /// <summary>
        /// 比較とかを1命令でやるために ulong 化。
        /// </summary>
        /// <remarks>
        /// 一応 little endian で読むようにしてる。
        /// </remarks>
        public ulong LongValue => BinaryPrimitives.ReadUInt64LittleEndian(this.AsSpan());

        public byte this[int index] => this.AsSpan()[index];

        public bool Equals(Byte8 other) => LongValue == other.LongValue;
        public override bool Equals(object? obj) => obj is Byte8 other && Equals(other);
        public override int GetHashCode() => LongValue.GetHashCode();
        public static bool operator ==(Byte8 x, Byte8 y) => x.Equals(y);
        public static bool operator !=(Byte8 x, Byte8 y) => !x.Equals(y);
    }

    internal static class Byte8Extensions
    {
        public static Span<byte> AsSpan(ref this Byte8 tags) => MemoryMarshal.CreateSpan(ref tags.V0, 8);
    }
}
