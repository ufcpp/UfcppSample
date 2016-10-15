using System;

namespace UtfString.ArrayImplementation.Utf8
{
    public static class Decoder
    {
        public static int GetByteCount(byte[] buffer)
        {
            var count = 0;
            foreach (var x in buffer)
                if ((x & 0b1100_0000) != 0b1000_0000)
                    count++;
            return count;
        }

        public static bool TryGetByteCount(byte[] buffer, int index, out byte byteCount)
        {
            byteCount = 0;

            if (index >= buffer.Length) return false;

            uint x = buffer[index];

            if (x < 0b1100_0000U)
            {
                byteCount = 1;
                return true;
            }

            if (x < 0b1110_0000U) byteCount = 2;
            else if (x < 0b1111_0000U) byteCount = 3;
            else byteCount = 4;

            if (index + byteCount > buffer.Length) return false;

            return true;
        }

        public static CodePoint Decode(byte[] buffer, Index index)
        {
            var i = index.index;
            uint x = buffer[i];
            uint code = 0;
            switch (index.byteCount)
            {
                case 1:
                    return new CodePoint(x);
                case 2:
                    code = x & 0b1_1111;
                    code = (code << 6) | (uint)(buffer[++i] & 0b0011_1111);
                    return new CodePoint(code);
                case 3:
                    code = x & 0b1111;
                    code = (code << 6) | (uint)(buffer[++i] & 0b0011_1111);
                    code = (code << 6) | (uint)(buffer[++i] & 0b0011_1111);
                    return new CodePoint(code);
                case 4:
                    code = x & 0b0111;
                    code = (code << 6) | (uint)(buffer[++i] & 0b0011_1111);
                    code = (code << 6) | (uint)(buffer[++i] & 0b0011_1111);
                    code = (code << 6) | (uint)(buffer[++i] & 0b0011_1111);
                    return new CodePoint(code);
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public static bool TryDecode(byte[] buffer, ref int index, out CodePoint codePoint)
        {
            codePoint = default(CodePoint);

            if (index >= buffer.Length) return false;

            uint code = buffer[index];

            if (code < 0b1100_0000)
            {
                // ASCII 文字
                codePoint = new CodePoint(code);
                ++index;
                return true;
            }
            if (code < 0b1110_0000)
            {
                // 2バイト文字
                if (index + 1 >= buffer.Length) return false;
                code &= 0b1_1111;
                code = (code << 6) | (uint)(buffer[++index] & 0b0011_1111);
                ++index;
                codePoint = new CodePoint(code);
                return true;
            }
            if (code < 0b1111_0000)
            {
                // 3バイト文字
                if (index + 2 >= buffer.Length) return false;
                code &= 0b1111;
                code = (code << 6) | (uint)(buffer[++index] & 0b0011_1111);
                code = (code << 6) | (uint)(buffer[++index] & 0b0011_1111);
                ++index;
                codePoint = new CodePoint(code);
                return true;
            }

            // 4バイト文字
            if (index + 3 >= buffer.Length) return false;
            code &= 0b0111;
            code = (code << 6) | (uint)(buffer[++index] & 0b0011_1111);
            code = (code << 6) | (uint)(buffer[++index] & 0b0011_1111);
            code = (code << 6) | (uint)(buffer[++index] & 0b0011_1111);
            ++index;
            codePoint = new CodePoint(code);
            return true;
        }
    }
}
