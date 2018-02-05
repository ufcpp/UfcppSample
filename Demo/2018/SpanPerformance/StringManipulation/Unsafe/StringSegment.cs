namespace StringManipulation.Unsafe
{
    /// <summary>
    /// 文字列中の一定区間。
    /// </summary>
    public unsafe readonly ref struct StringSegment
    {
        /// <summary>
        /// 元の文字列。
        /// </summary>
        public readonly char* Pointer;

        /// <summary>
        /// 区間の長さ。
        /// </summary>
        public readonly int Length;

        public StringSegment(char* p, int length)
        {
            Pointer = p;
            Length = length;
        }

        public char this[int index] => Pointer[index];

        public override string ToString() =>
            Pointer == null ? null :
            Length == 0 ? "" : 
            new string (Pointer, 0, Length);
    }
}
