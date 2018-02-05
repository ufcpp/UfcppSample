namespace StringManipulation.Unsafe
{
    /// <summary>
    /// <see cref="string.Split(char[])"/>みたいに、文字列を分割して文字列の配列を作る処理を no allocation で行うためのインターフェイス。
    /// </summary>
    /// <remarks>
    /// ref struct は直接インターフェイスを実装できないのでちょっと回りくどい手段が必要。
    ///
    /// ↓みたいに内部に Span を持ってしまうとインターフェイスを実装できなくなるので、ref 引数で Span を渡してもらう。
    /// <code><![CDATA[
    /// ref struct Splitter
    /// {
    ///     StringSpan _s;
    /// }
    /// ]]></code>
    /// </remarks>
    public interface IStringSplitter
    {
        /// <summary>
        /// 1ワード切り出し。
        /// </summary>
        /// <param name="state">元文字列。切り出した分進めた Span で上書き。</param>
        /// <param name="nextWord">切り出した部分文字列。</param>
        /// <returns>切り出せたら true。</returns>
        bool TryMoveNext(ref StringSpan state, out StringSpan nextWord);
    }
}
