using System.Runtime.InteropServices;

namespace RgiSequenceFinder
{
    /// <summary>
    /// RGI 絵文字シーケンス判定の結果。
    /// </summary>
    /// <remarks>
    /// keycap と国旗は特殊なので別テーブル参照したく、単に文字列長だけじゃなくて <see cref="EmojiSequenceType"/> を一緒に返す。
    ///
    /// keycap と国旗の時には
    /// <see cref="RgiSequenceFinder.Keycap"/>、<see cref="RegionalIndicator"/>、<see cref="TagSequence"/>
    /// を使って ASCII (byte 列)ベースのテーブル引きをするのでこれも一緒に返す。
    ///
    /// 絵文字判定を受けなかった場合は <see cref="NotEmoji"/>、
    /// 空文字のときは default。
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct EmojiSequence
    {
        [FieldOffset(0)]
        public readonly EmojiSequenceType Type;

        [FieldOffset(4)]
        public readonly int LengthInUtf16;

        [FieldOffset(8)]
        public readonly Keycap Keycap;

        [FieldOffset(8)]
        public readonly RegionalIndicator Region;

        [FieldOffset(8)]
        public readonly Byte8 Tags;

        public EmojiSequence(EmojiSequenceType type, int length) : this()
        {
            Type = type;
            LengthInUtf16 = length;
        }

        public EmojiSequence(Keycap keycap) : this(EmojiSequenceType.Keycap, 3)
        {
            Keycap = keycap;
        }

        public EmojiSequence(RegionalIndicator region) : this(EmojiSequenceType.Flag, 4)
        {
            Region = region;
        }

        public EmojiSequence(int tagLength, Byte8 tags)
            : this(
                  tagLength > TagSequence.TagMaxLength ? EmojiSequenceType.MoreBufferRequired : EmojiSequenceType.Tag,
                  2 * tagLength + 2)
        {
            Tags = tags;
        }

        /// <summary>
        /// 絵文字シーケンス判定を受けなかった1文字。
        /// default が長さ0 (空文字列、文末)を表すのに対してこっちは <see cref="LengthInUtf16"/> が1。
        /// </summary>
        public static readonly EmojiSequence NotEmoji = new(EmojiSequenceType.NotEmoji, 1);

        public void Deconstruct(out EmojiSequenceType type, out int lengthInUtf16) => (type, lengthInUtf16) = (Type, LengthInUtf16);
    }
}
