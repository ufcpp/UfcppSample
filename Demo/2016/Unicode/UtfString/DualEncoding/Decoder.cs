namespace UtfString.DualEncoding
{
    public static class Decoder
    {
        public static int GetLength(ArrayAccessor buffer)
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

        public static byte TyrGetCount(ArrayAccessor buffer, int index)
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

        public static CodePoint Decode(ArrayAccessor buffer, Index index)
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

        public static (CodePoint cp, byte count) TryDecode(ArrayAccessor buffer, int index)
        {
            if (index >= buffer.Length) return Constants.End;

            uint x = buffer[index++];

            if ((x & 0b1111_1100_0000_0000) == 0b1101_1000_0000_0000)
            {
                var code = (x & 0b0011_1111_1111) + 0b0100_0000;
                if (index >= buffer.Length) return Constants.End;
                x = buffer[index++];
                if ((x & 0b1111_1100_0000_0000) != 0b1101_1100_0000_0000) return Constants.End;
                code = (code << 10) | (x & 0b0011_1111_1111);

                return (new CodePoint(code), 2);
            }
            else
            {
                return (new CodePoint(x), 1);
            }
        }
    }
}
