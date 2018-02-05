using System;

namespace StringManipulation.SafeStackalloc
{
    /// <summary>
    /// <see cref="Unsafe.IStringSplitter"/>
    /// </summary>
    public interface IStringSplitter
    {
        /// <summary>
        /// 1ワード切り出し。
        /// </summary>
        /// <param name="state">元文字列。切り出した分進めた Span で上書き。</param>
        /// <param name="nextWord">切り出した部分文字列。</param>
        /// <returns>切り出せたら true。</returns>
        bool TryMoveNext(ref ReadOnlySpan<char> state, out ReadOnlySpan<char> nextWord);
    }
}
