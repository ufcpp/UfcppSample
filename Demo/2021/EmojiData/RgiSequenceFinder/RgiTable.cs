using System;

namespace RgiSequenceFinder
{
    internal partial class RgiTable
    {
        /// <summary>
        /// いったん、1絵文字1インデックス想定な構造で作成。
        /// 実際には ZWJ シーケンスの場合、「見つからなかったら ZWJ でスプリットしてから再検索」とかやるので、
        /// 1絵文字が複数のインデックスになる予定。
        /// </summary>
        public static (int charRead, int index) Find(ReadOnlySpan<char> s)
        {
            var emoji = GraphemeBreak.GetEmojiSequence(s);

            var index = emoji.Type switch
            {
                EmojiSequenceType.Other => FindOther(s.Slice(0, emoji.LengthInUtf16)),
                EmojiSequenceType.Flag => FindRegion(emoji.Region),
                EmojiSequenceType.Tag => FindTag(emoji.Tags),
                EmojiSequenceType.Keycap => FindKeycap(emoji.Keycap),
                _ => -1,
                // MoreBufferRequired の時は throw する？
            };

            //todo: -1 の時の再検索
            // - ZWJ で分割して再検索
            // - FE0F(異体字セレクター16)を消してみて再検索
            // - 1F3FB～1F3FF (肌色選択)を消してみて再検索 + 肌色自体の絵
            //
            // テストに使えそうな絵文字:
            // - Windows オリジナルキャラ: 🐱‍👤🐱‍🏍🐱‍💻🐱‍🐉🐱‍👓🐱‍🚀
            // 1F431 200D の後ろにそれぞれ 1F464, 1F3CD, 1F4BB, 1F409, 1F453, 1F680
            // - Windows は頑張ってレンダリングしてる4人家族×肌色: 👩🏻‍👩🏿‍👧🏼‍👧🏾
            // 1F469 1F3FB 200D 1F469 1F3FF 200D 1F467 1F3FC 200D 1F467 1F3FE

            return (emoji.LengthInUtf16, index);
        }
    }
}
