using System;

namespace DiscriminatedUnion.IsMatching
{
    /// <summary>
    /// string or char[]。
    /// <see cref="Span"/> の実装に普通に is 演算子を使う。
    /// </summary>
    /// <remarks>
    /// is によるパターンマッチングは、実際のところ、
    /// <code><![CDATA[
    /// var temp = _value as string;
    /// if (temp != null) ...
    /// ]]></code>
    /// みたいなコードと同値。
    /// で、as 演算子は IL 的には isinst 命令になってる。
    ///
    /// isinst 命令は要は実行時型情報を調べる命令。
    /// 実行時型情報と言っても、動的コード生成をしない(単に型を調べるだけ)ならそこまで高コストではない。
    /// なので、動的コード生成みたいに「静的なコードに比べて2桁遅い」みたいな事態にはならない。
    /// </remarks>
    public readonly struct StringOrCharArray
    {
        private readonly object _value;

        public StringOrCharArray(string s) => _value = s;
        public StringOrCharArray(char[] array) => _value = array;

        public ReadOnlySpan<char> Span
            => _value is string s ? s.AsSpan() :
            _value is char[] a ? a.AsSpan() :
            default;
    }
}
