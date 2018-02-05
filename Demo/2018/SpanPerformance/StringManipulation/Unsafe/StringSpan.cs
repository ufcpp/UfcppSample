namespace StringManipulation.Unsafe
{
    /// <summary>
    /// 文字列中の一定区間。
    /// </summary>
    /// <remarks>
    /// <see cref="System.Span{T}"/> を持っているわけじゃないので ref struct にしなくてもこの型自体のコンパイルはできるんだけども。
    /// 用途的には Span と同じで、ref struct にしておかないとほんとに unsafe なので。
    /// </remarks>
    public unsafe readonly ref struct StringSpan
    {
        /// <summary>
        /// 元の文字列。
        /// </summary>
        public readonly char* Pointer;

        /// <summary>
        /// 区間の長さ。
        /// </summary>
        public readonly int Length;

        public StringSpan(char* p, int length)
        {
            Pointer = p;
            Length = length;
        }

        // せっかく unsafe なんだし範囲チェックさぼる。
        public ref char this[int index] => ref Pointer[index];

        public StringSpan Slice(int startIndex) => new StringSpan(Pointer + startIndex, Length - startIndex);
        public StringSpan Slice(int startIndex, int length) => new StringSpan(Pointer + startIndex, length);

        public override string ToString() =>
            Pointer == null ? null :
            Length == 0 ? "" : 
            new string (Pointer, 0, Length);
    }
}
