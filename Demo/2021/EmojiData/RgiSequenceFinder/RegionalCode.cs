using System;

namespace RgiSequenceFinder
{
    /// <summary>
    /// Regional Indicator 2文字を単純な数値化したもの。
    /// </summary>
    /// <remarks>
    /// 国旗は Regional Indicator っていう A～Z に相当する特殊記号26文字があって、それ2個(固定長)の組み合わせで表現されてる。
    /// 原理上26×26が上限(これよりも複雑な国旗は emoji tag sequence っていう別の仕様を使って表現することになってる)。
    ///
    /// なので、(s[0] - 'A') * 26 + (s[1] - 'A') みたいな計算で 0～26×26 の数値化できるのでやっちゃうことにする。
    /// </remarks>
    public readonly struct RegionalCode
    {
        public readonly short Value;
        public RegionalCode(char firstLowSurrogate, char secondLowSurrogate)
        {
            const int RegionalIndicatorSymbolA = 0xDDE6;
            var f = firstLowSurrogate - RegionalIndicatorSymbolA;
            var s = secondLowSurrogate - RegionalIndicatorSymbolA;

            Value = (short)(f * 26 + s);
        }

        private RegionalCode(short s) => Value = s;

        /// <summary>
        /// 「国旗じゃなかった」判定結果に -1 の値を使う。
        /// </summary>
        public static readonly RegionalCode Invalid = new(-1);

        public override string ToString()
        {
            var (f, s) = Math.DivRem((int)Value, 26);
            return $"{(char)(f + 'A')}{(char)(s + 'A')}";
        }
    }
}
