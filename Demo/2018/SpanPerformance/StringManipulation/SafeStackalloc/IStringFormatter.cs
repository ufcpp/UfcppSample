using System;

namespace StringManipulation.SafeStackalloc
{
    /// <summary>
    /// <see cref="Unsafe.IStringFormatter"/>
    /// </summary>
    public interface IStringFormatter
    {
        /// <summary>
        /// 1ワード書き込み。
        /// </summary>
        /// <param name="word">書き込みたい文字列。</param>
        /// <param name="buffer">書き込み先。書き込んだ分進めた span で上書き。</param>
        void Write(ReadOnlySpan<char> word, ref Span<char> buffer);
    }
}
