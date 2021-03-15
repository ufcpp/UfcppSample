namespace RgiSequenceFinder
{
    /// <summary>
    /// 絵文字シーケンスの grapheme cluster 分割の種類。
    /// 「絵文字じゃなかった」判定も兼ねてる。
    /// </summary>
    /// <remarks>
    /// 分けるとテーブル引きが楽なやつだけ別枠にする。
    /// <see cref="Other"/> というけど、大半の文字が other。
    /// </remarks>
    public enum EmojiSequenceType
    {
        /// <summary>
        /// 絵文字じゃなかった。
        /// </summary>
        NotEmoji,

        /// <summary>
        /// basic emoji とか emoji ZWJ sequence とか、普通に文字列比較でテーブル引くやつ。
        /// 大部分がこれ。
        /// </summary>
        Other,

        /// <summary>
        /// keycap sequence。
        /// 0-9, *. # だけで判定できる。
        /// </summary>
        Keycap,

        /// <summary>
        /// Regional Indicator 2文字の国旗。
        /// Regional Indicator に相当する ASCII 2文字でテーブル引く想定。
        /// </summary>
        Flag,

        /// <summary>
        /// emoji tag sequence。
        /// Tag 文字に相当する ASCII 数文字でテーブル引く想定。
        /// </summary>
        Tag,

        /// <summary>
        /// 想定よりも長い絵文字シーケンスが来て対応できないとき。
        /// </summary>
        /// <remarks>
        /// 現行 RGI だと <see cref="Tag"/> は3種類しかなくて、長さは Cancel Tag を含めても6文字しかこないはずなので、
        /// ASCII 文字を入れておくバッファーを固定長にするつもり。
        ///
        /// 一方で、意図的にもっと長い tag sequence を送り付けてバッファーオーバーランを狙えるので、そういう文字列が来た時にエラーにする用の enum 値。
        /// </remarks>
        MoreBufferRequired,
    }
}
