namespace RgiSequenceFinder
{
    /// <summary>
    /// Regional Indicator 2文字を単純な数値化したもの。
    /// </summary>
    /// <remarks>
    /// 国旗は Regional Indicator っていう A～Z に相当する特殊記号26文字があって、それ2個(固定長)の組み合わせで表現されてる。
    /// 原理上26×26が上限(これよりも複雑な国旗は emoji tag sequence っていう別の仕様を使って表現することになってる)。
    ///
    /// 結局、A-Z の ASCII 文字をそのまま使うことにしたので byte ×2 なデータ構造に。
    /// </remarks>
    public readonly struct RegionalIndicator
    {
        public readonly byte First;
        public readonly byte Second;

        public RegionalIndicator(char firstLowSurrogate, char secondLowSurrogate)
        {
            const int RegionalIndicatorSymbolA = 0xDDE6;
            First = (byte)(firstLowSurrogate - RegionalIndicatorSymbolA + 'A');
            Second = (byte)(secondLowSurrogate - RegionalIndicatorSymbolA + 'A');
        }

        public void Deconstruct(out byte first, out byte second) => (first, second) = (First, Second);

        /// <summary>
        /// 「国旗じゃなかった」判定結果に 0, 0 の値を使う。
        /// </summary>
        public static readonly RegionalIndicator Invalid = default;

        public override string ToString()
        {
            var (f, s) = this;
            return $"{(char)f}{(char)s}";
        }
    }
}
