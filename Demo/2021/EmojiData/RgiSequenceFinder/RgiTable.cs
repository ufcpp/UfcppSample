using System;

namespace RgiSequenceFinder
{
    internal partial class RgiTable
    {
        /// <summary>
        /// <see cref="GraphemeBreak"/> 的に 1 grapheme 判定を受けてるシーケンスが、絵文字表示的には複数文字になることがある。
        /// いったん、1絵文字1インデックス想定な構造で作成。
        /// 実際には ZWJ シーケンスの場合、「見つからなかったら ZWJ でスプリットしてから再検索」とかやるので、
        /// 1絵文字が複数のインデックスになる予定。
        /// </summary>
        /// <param name="s">絵文字シーケンスを検出したい文字列。</param>
        /// <param name="indexes">表示する絵文字画像のインデックスの書き込み先。</param>
        /// <returns>
        /// charRead: <paramref name="s"/> の先頭なん文字を読んだか(UTF-16 長)。
        /// indexWritten: <paramref name="indexes"/> に何文字書き込んだか。 RGI 絵文字シーケンスが見つからなかった時は0。
        /// </returns>
        public static (int charRead, int indexWritten) Find(ReadOnlySpan<char> s, Span<int> indexes)
        {
            var emoji = GraphemeBreak.GetEmojiSequence(s);

            switch (emoji.Type)
            {
                case EmojiSequenceType.Other:
                    {
                        var span = s.Slice(0, emoji.LengthInUtf16);
                        var i = FindOther(span);

                        if (i >= 0)
                        {
                            indexes[0] = i;
                            return (emoji.LengthInUtf16, 1);
                        }

                        var written = ReduceExtends(span, indexes);
                        
                        return (emoji.LengthInUtf16, written);
                    }
                case EmojiSequenceType.Keycap:
                    {
                        var i = FindKeycap(emoji.Keycap);
                        if (i < 0) return (3, 0);
                        indexes[0] = i;
                        return (3, 1);
                    }
                case EmojiSequenceType.Flag:
                    {
                        var i = FindRegion(emoji.Region);

                        //todo: 見つからなかった時、ASCII 化する？
                        // AA (対応する地域コードなし)を "AA" に置き換えるみたいなの。

                        if (i < 0) return (4, 0);

                        indexes[0] = i;
                        return (4, 1);
                    }
                case EmojiSequenceType.Tag:
                    {
                        var i = FindTag(emoji.Tags);

                        // 見つからなかった時、タグ文字を削って再検索する。
                        // 例えば 1F3F4 E006A E0070 E0031 E0033 E007F (原理的にはあり得る「東京都(JP13)の旗」)を 1F3F4 (🏴) だけにして返すみたいなの。
                        //
                        // http://unicode.org/reports/tr51/#DisplayInvalidEmojiTagSeqs 曰く、
                        // 推奨としては、「未サポートな旗」であることがわかるように黒旗に ? マークを重ねるか、黒旗 + ? で表示しろとのこと。
                        // ただ、タグ文字を解釈できないときには黒旗だけの表示も認めてそう。
                        //
                        // この実装では黒旗だけの表示をすることにする。
                        if (i < 0) i = FindOther(s.Slice(0, 2));

                        if (i < 0) return (emoji.LengthInUtf16, 0);

                        indexes[0] = i;
                        return (emoji.LengthInUtf16, 1);
                    }
                case EmojiSequenceType.SkinTone:
                    {
                        var i = FindSkinTone(emoji.SkinTone);
                        indexes[0] = i;
                        return (2, 1);
                    }
                default:
                case EmojiSequenceType.NotEmoji:
                case EmojiSequenceType.MoreBufferRequired:
                    // MoreBufferRequired の時は throw する？
                    return (emoji.LengthInUtf16, 0);
            }
        }

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

        /// <summary>
        /// Extend (FE0F と skin tone) 削り。
        /// FE0F → ただ消す。
        /// skin tone → 基本絵文字 + 肌色四角に分解。
        /// </summary>
        private static int ReduceExtends(ReadOnlySpan<char> s, Span<int> indexes)
        {
            // variation selector 16 削り。
            // FE0F (variation selector 16)は「絵文字扱いする」という意味なので、
            // RGI 的には FE0F なしで絵文字になってるものに余計に FE0F がくっついてても絵文字扱いしていい。
            if (s[s.Length - 1] == '\uFE0F')
            {
                // Find から再起するか(国旗 + FE0F とか、FE0F 複数個並べるとかに対応)までやるかどうか…
                var i = FindOther(s.Slice(0, s.Length - 1));

                if (i >= 0)
                {
                    if (indexes.Length > 0) indexes[0] = i;
                    return 1;
                }
            }

            // 肌色。
            // skin tone よりも後ろに ZWJ を挟まず何かがくっついてることないはず。
            // この実装ではあっても無視。
            // 2個以上 skin tone が並んでるとかも無視。
            // 間に FEOF が挟まってる場合とかも未サポート。
            // BMP + skin tone の可能性
            if (s.Length >= 3)
            {
                var st = GraphemeBreak.IsSkinTone(s.Slice(1));
                if (st >= 0)
                {
                    var i = FindOther(s.Slice(0, 1));

                    if (i < 0)
                    {
                        if (indexes.Length > 0) indexes[0] = FindSkinTone(st);
                        return 1;
                    }
                    else
                    {
                        if (indexes.Length > 0) indexes[0] = i;
                        if (indexes.Length > 1) indexes[1] = FindSkinTone(st);
                        return 2;
                    }
                }
            }

            // SMP + skin tone の可能性
            if (s.Length >= 4)
            {
                var st = GraphemeBreak.IsSkinTone(s.Slice(2));
                if (st >= 0)
                {
                    var i = FindOther(s.Slice(0, 2));

                    if (i < 0)
                    {
                        if (indexes.Length > 0) indexes[0] = FindSkinTone(st);
                        return 1;
                    }
                    else
                    {
                        if (indexes.Length > 0) indexes[0] = i;
                        if (indexes.Length > 1) indexes[1] = FindSkinTone(st);
                        return 2;
                    }
                }
            }

            return 0;
        }
    }
}
