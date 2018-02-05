namespace StringManipulation.Unsafe
{
    /// <summary>
    /// <see cref="IStringSplitter"/>の逆。
    /// 1つの Span に対して文字列を書き込んでいく。
    /// </summary>
    public interface IStringFormatter
    {
        /// <summary>
        /// 1ワード書き込み。
        /// </summary>
        /// <param name="word">書き込みたい文字列。</param>
        /// <param name="buffer">書き込み先。書き込んだ分進めた span で上書き。</param>
        void Write(StringSpan word, ref StringSpan buffer);
    }
}
