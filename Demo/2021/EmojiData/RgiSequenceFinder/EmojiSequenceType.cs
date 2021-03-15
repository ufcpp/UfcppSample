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
        NotEmoji,
        Other,
        Keycap,
        Flag,
        Tag,
    }
}
