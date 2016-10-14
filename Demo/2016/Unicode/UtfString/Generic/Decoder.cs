using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UtfString.Generic
{
    public interface IDecoder<TChar, ArrayAccessor>
        where TChar : struct
        where ArrayAccessor : struct, IArrayAccessor<TChar>
    {
        int GetLength(ArrayAccessor buffer);
        byte TyrGetCount(ArrayAccessor buffer, int index);
        CodePoint Decode(ArrayAccessor buffer, Index index);
        (CodePoint cp, byte count) TryDecode(ArrayAccessor buffer, int index);
    }

    public struct Utf32Decoder : IDecoder<uint, IntAccessor>
    {
        public int GetLength(IntAccessor buffer) => buffer.Length;
        public byte TyrGetCount(IntAccessor buffer, int index)
        {
            if (index >= buffer.Length) return Constants.InvalidCount;
            return 1;
        }

        public CodePoint Decode(IntAccessor buffer, Index index) => new CodePoint(buffer[index.index]);

        public (CodePoint cp, byte count) TryDecode(IntAccessor buffer, int index)
        {
            if (index >= buffer.Length) return Constants.End;
            return (new CodePoint(buffer[index]), 1);
        }
    }

    public struct Utf16Decoder<ArrayAccessor> : IDecoder<ushort, ArrayAccessor>
        where ArrayAccessor : struct, IArrayAccessor<ushort>
    {
        public int GetLength(ArrayAccessor buffer)
        {
            var count = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                var x = buffer[i];
                if ((x & 0b1111_1100_0000_0000) != 0b1101_1100_0000_0000)
                    count++;
            }
            return count;
        }

        public byte TyrGetCount(ArrayAccessor buffer, int index)
        {
            if (index >= buffer.Length) return Constants.InvalidCount;
            uint x = buffer[index];
            if ((x & 0b1111_1100_0000_0000) == 0b1101_1000_0000_0000)
            {
                if (index + 1 >= buffer.Length) return Constants.InvalidCount;
                return 2;
            }
            else
                return 1;
        }

        public CodePoint Decode(ArrayAccessor buffer, Index index)
        {
            var i = index.index;
            uint x = buffer[i];

            if (index.count == 2)
            {
                uint y = buffer[i + 1];

                var code = (x & 0b0011_1111_1111) + 0b0100_0000;
                code = (code << 10) | (y & 0b0011_1111_1111);
                return new CodePoint(code);
            }
            else
            {
                return new CodePoint(x);
            }
        }

        public (CodePoint cp, byte count) TryDecode(ArrayAccessor buffer, int index)
        {
            if (index >= buffer.Length) return Constants.End;

            uint x = buffer[index++];

            if ((x & 0b1111_1100_0000_0000) == 0b1101_1000_0000_0000)
            {
                // サロゲート ペアの処理
                var code = (x & 0b0011_1111_1111) + 0b0100_0000;
                if (index >= buffer.Length) return Constants.End;
                x = buffer[index++];
                if ((x & 0b1111_1100_0000_0000) != 0b1101_1100_0000_0000) return Constants.End;
                code = (code << 10) | (x & 0b0011_1111_1111);

                return (new CodePoint(code), 2);
            }
            else
            {
                // 利用頻度が高い文字はほぼこちら側に来る
                // バッファー内の値を素通し。
                return (new CodePoint(x), 1);
            }
        }
    }

    public struct Utf8Decoder : IDecoder<byte, ByteAccessor>
    {
        public int GetLength(ByteAccessor buffer)
        {
            var count = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                var x = buffer[i];
                if ((x & 0b1100_0000) != 0b1000_0000)
                    count++;
            }
            return count;
        }

        public byte TyrGetCount(ByteAccessor buffer, int index)
        {
            if (index >= buffer.Length) return Constants.InvalidCount;

            uint x = buffer[index];

            var byteCount =
                (x >= 0b1111_0000U) ? (byte)4 :
                (x >= 0b1110_0000U) ? (byte)3 :
                (x >= 0b1100_0000U) ? (byte)2 :
                (byte)1;

            if (index + byteCount > buffer.Length) return Constants.InvalidCount;

            return byteCount;
        }

        public CodePoint Decode(ByteAccessor buffer, Index index)
        {
            var i = index.index;
            uint x = buffer[i++];
            uint code = 0;
            switch (index.count)
            {
                case 1:
                    return new CodePoint(x);
                case 2:
                    code = x & 0b1_1111;
                    code = (code << 6) | (uint)(buffer[i++] & 0b0011_1111);
                    return new CodePoint(code);
                case 3:
                    code = x & 0b1111;
                    code = (code << 6) | (uint)(buffer[i++] & 0b0011_1111);
                    code = (code << 6) | (uint)(buffer[i++] & 0b0011_1111);
                    return new CodePoint(code);
                case 4:
                    code = x & 0b0111;
                    code = (code << 6) | (uint)(buffer[i++] & 0b0011_1111);
                    code = (code << 6) | (uint)(buffer[i++] & 0b0011_1111);
                    code = (code << 6) | (uint)(buffer[i++] & 0b0011_1111);
                    return new CodePoint(code);
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public (CodePoint cp, byte count) TryDecode(ByteAccessor buffer, int index)
        {
            if (index >= buffer.Length) return Constants.End;

            uint code = buffer[index++];

            if (code >= 0b1111_0000)
            {
                // 4バイト文字
                code &= 0b0111;
                if (!TryNext(buffer, ref index, ref code)) return Constants.End;
                if (!TryNext(buffer, ref index, ref code)) return Constants.End;
                if (!TryNext(buffer, ref index, ref code)) return Constants.End;
                return (new CodePoint(code), 4);
            }
            if (code >= 0b1110_0000)
            {
                // 3バイト文字
                code &= 0b1111;
                if (!TryNext(buffer, ref index, ref code)) return Constants.End;
                if (!TryNext(buffer, ref index, ref code)) return Constants.End;
                return (new CodePoint(code), 3);
            }
            if (code >= 0b1100_0000)
            {
                // 2バイト文字
                code &= 0b1_1111;
                if (!TryNext(buffer, ref index, ref code)) return Constants.End;
                return (new CodePoint(code), 2);
            }

            // ASCII 文字
            return (new CodePoint(code), 1);
        }

        private bool TryNext(ByteAccessor buffer, ref int index, ref uint code)
        {
            if (index >= buffer.Length) return false;

            var c = buffer[index++];
            if ((c & 0b1100_0000) != 0b1000_0000) return false;

            code = (code << 6) | (uint)(c & 0b0011_1111);
            return true;
        }
    }
}
