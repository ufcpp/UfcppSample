namespace UtfString.Utf32
{
    public static class Decoder
    {
        public static int GetLength(ArrayAccessor buffer) => buffer.Length;
        public static byte TyrGetCount(ArrayAccessor buffer, int index)
        {
            if (index >= buffer.Length) return Constants.InvalidCount;
            return 1;
        }

        public static CodePoint Decode(ArrayAccessor buffer, Index index) => new CodePoint(buffer[index.index]);

        public static (CodePoint cp, byte count) TryDecode(ArrayAccessor buffer, int index)
        {
            if (index >= buffer.Length) return Constants.End;
            return (new CodePoint(buffer[index]), 1);
        }
    }
}
