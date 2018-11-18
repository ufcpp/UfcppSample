using System;
using System.Runtime.CompilerServices;

namespace DiscriminatedUnion.DiscriminatorFieldUnsafe
{
    /// <summary>
    /// string or char[]。
    /// <see cref="Span"/> の実装に is 演算子を避けて、<see cref="Unsafe"/> を使うことで高速化した実装。
    /// </summary>
    /// <remarks>
    /// <see cref="DiscriminatorField.StringOrCharArray"/> の方で書いている通り、
    /// enum 値の比較で isinst を避けたのに、結局キャストするのでは意味がない。
    ///
    /// そこで、<see cref="Unsafe.As{TFrom, TTo}(ref TFrom)"/> を使って強制型変換してしまう。
    /// こいつは、型チェックをすっとばしてるので高速。
    /// 名前通り unsafe ではあるものの、事前に enum 値で型を調べているので問題は起こさない。
    ///
    /// これで無事、isinst 命令の負担を避けれるので高速。
    /// <see cref="IsMatching.StringOrCharArray"/> で書いたように桁違いに速くなったりはしないものの、
    /// 数割程度は高速。
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
                // せっかく Type を見て switch してるんだから
                switch (Type)
                {
                    default: return default;
                    // キャストを Unsafe.As で置き換えれば高速。
                    case Discriminator.String: return Unsafe.As<object, string>(ref Unsafe.AsRef(_value)).AsSpan();
                    case Discriminator.CharArray: return Unsafe.As<object, char[]>(ref Unsafe.AsRef(_value)).AsSpan();
                }
            }
        }
    }
}
