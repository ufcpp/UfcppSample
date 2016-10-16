namespace UtfString.Slices
{
    public static class Decoder
    {
        public static int GetByteCount(ReadOnlySpan buffer)
        {
            var count = 0;
            foreach (var x in buffer)
                if ((x & 0b1100_0000) != 0b1000_0000)
                    count++;
            return count;
        }

        public static bool TryDecode(ReadOnlySpan buffer, ref int index, out CodePoint codePoint)
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
