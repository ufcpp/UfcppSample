using System;

namespace DiscriminatedUnion.DiscriminatorField
{
    /// <summary>
    /// string or char[]。
    /// <see cref="Span"/> の実装に普通に is 演算子を避けようと試みてるけど… みたいな実装。
    /// </summary>
    /// <remarks>
    /// <see cref="IsMatching.StringOrCharArray"/> の方で書いている通り、isinst 命令はそこまで遅くもないんだけど。
    /// 整数値の比較よりは遅いらしく、isinst を避けるために弁別用の enum フィールドを持とうという実装。
    ///
    /// ただ、結局 object のフィールドから string、もしくは、char[] を読み出そうとしたときにキャストが必要。
    /// キャストの方は castclass 命令になるんだけど、内部的に isinst 命令と大差ないみたいで、実行時間もほとんど同じ。
    ///
    /// となると、このクラスは実は失敗作で、
    /// 「せっかく事前に enum 値で型を判定してるのに、castclass 内で改めて型チェックをしてて、単に2重の負担がかかってるだけ」
    /// になる。
    /// もちろん、ちょっとだけ <see cref="IsMatching.StringOrCharArray"/> よりも遅くなる。
    /// </remarks>
    public readonly struct StringOrCharArray
    {
        public Discriminator Type { get; }
        private readonly object _value;

        public StringOrCharArray(string s) => (Type, _value) = (Discriminator.String, s);
        public StringOrCharArray(char[] array) => (Type, _value) = (Discriminator.CharArray, array);

        public ReadOnlySpan<char> Span
        {
            get
            {
                // せっかく Type を見て switch してるのに…
                switch (Type)
                {
                    default: return default;
                    // この2行のキャストが余計。
                    case Discriminator.String: return ((string)_value).AsSpan();
                    case Discriminator.CharArray: return ((char[])_value).AsSpan();
                }
            }
        }
    }
}
