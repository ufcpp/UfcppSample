using System;

namespace UtfString.Utf8
{
    public static class Decoder
    {
        public static int GetLength(ArrayAccessor buffer)
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

        public static byte TyrGetCount(ArrayAccessor buffer, int index)
        {
            if (index >= buffer.Length) return Constants.InvalidCount;

            uint x = buffer[index];

            var byteCount =
                (x < 0b1100_0000U) ? (byte)1 :
                (x < 0b1110_0000U) ? (byte)2 :
                (x < 0b1111_0000U) ? (byte)3 :
                (byte)4;

            if (index + byteCount > buffer.Length) return Constants.InvalidCount;

            return byteCount;
        }

        public static CodePoint Decode(ArrayAccessor buffer, Index index)
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

        public static (CodePoint cp, byte count) TryDecode(ArrayAccessor buffer, int index)
        {
            if (index >= buffer.Length) return Constants.End;

            uint code = buffer[index];

            if (code < 0b1100_0000)
            {
                // ASCII 文字
                return (new CodePoint(code), 1);
            }
            if (code < 0b1110_0000)
            {
                // 2バイト文字
                if (index + 1 >= buffer.Length) return Constants.End;
                code &= 0b1_1111;
                code = (code << 6) | (uint)(buffer[++index] & 0b0011_1111);
                return (new CodePoint(code), 2);
            }
            if (code < 0b1111_0000)
            {
                // 3バイト文字
                if (index + 2 >= buffer.Length) return Constants.End;
                code &= 0b1111;
                code = (code << 6) | (uint)(buffer[++index] & 0b0011_1111);
                code = (code << 6) | (uint)(buffer[++index] & 0b0011_1111);
                return (new CodePoint(code), 3);
            }

            // 4バイト文字
            if (index + 3 >= buffer.Length) return Constants.End;
            code &= 0b0111;
            code = (code << 6) | (uint)(buffer[++index] & 0b0011_1111);
            code = (code << 6) | (uint)(buffer[++index] & 0b0011_1111);
            code = (code << 6) | (uint)(buffer[++index] & 0b0011_1111);
            return (new CodePoint(code), 4);
        }

        private static bool TryNext(ArrayAccessor buffer, ref int index, ref uint code)
        {
            if (index >= buffer.Length) return false;

            var c = buffer[index++];
            if ((c & 0b1100_0000) != 0b1000_0000) return false;

            code = (code << 6) | (uint)(c & 0b0011_1111);
            return true;
        }
    }
}
