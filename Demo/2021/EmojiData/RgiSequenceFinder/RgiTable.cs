using System;

namespace RgiSequenceFinder
{
    internal partial class RgiTable
    {
        /// <summary>
        /// <see cref="GraphemeBreak"/> 的に 1 grapheme 判定を受けてるシーケンスが、絵文字表示的には複数文字になることがある。
        /// いったん、1絵文字1インデックス想定な構造で作成。
        /// 実際には ZWJ シーケンスの場合、「見つからなかったら ZWJ でスプリットしてから再検索」とかやるので、1絵文字が複数のインデックスになる。
        /// </summary>
        /// <param name="s">絵文字シーケンスを検出したい文字列。</param>
        /// <param name="indexes">表示する絵文字画像のインデックスの書き込み先。</param>
        /// <returns>
        /// charRead: <paramref name="s"/> の先頭なん文字を読んだか(UTF-16 長)。
        /// indexWritten: <paramref name="indexes"/> に何文字書き込んだか。 RGI 絵文字シーケンスが見つからなかった時は0。
        /// </returns>
        /// <remarks>
        /// 要検討:
        /// 今のところ「テーブル中になければ -1」とか「無視」とかやってるんだけど、
        /// 1符号点の時は Rune.Value を返してもいいかも。
        /// インデックスと混ざらないように ~Rune.Value とかにして。
        ///
        /// 例えば、🐱‍🏍 は 1F431 200D 1F3CD の3符号点からなる非 RGI 絵文字なんだけど、
        /// 1F431 (🐱) はいいとして、1F3CD (🏍) が「FE0D が付いてるときだけ絵文字扱い」な文字になってて、RGI テーブル中にない。
        /// こういうときに ~1F3CD を返しとくとかやってもいいかも。
        ///
        /// あと、非 RGI flag sequence とかも、1F1E6 1F1E6 (AA) に対して ASCII の "AA" (0041 0041) を表示してもいいんだけど、現状それができない。
        /// これも、 ~41 ~41 とかを返すようにすれば "AA" への復元は可能。
        /// </remarks>
        public static (int charRead, int indexWritten) Find(ReadOnlySpan<char> s, Span<int> indexes)
        {
            var emoji = GraphemeBreak.GetEmojiSequence(s);

            switch (emoji.Type)
            {
                case EmojiSequenceType.Other:
                    {
                        var span = s.Slice(0, emoji.LengthInUtf16);
                        var i = FindOther(span, emoji.ZwjPositions.SkinTones);

                        if (i >= 0)
                        {
                            indexes[0] = i;
                            return (emoji.LengthInUtf16, 1);
                        }

                        // 素直に見つからなかったときの再検索
                        // - ZWJ で分割して再検索
                        // - FE0F(異体字セレクター16)を消してみて再検索
                        // - 1F3FB～1F3FF (肌色選択)を消してみて再検索 + 肌色自体の絵
                        var written = SplitZqjSequence(emoji.ZwjPositions, span, indexes);
                        
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

        /// <summary>
        /// RGI にない ZWJ sequence が来た時、ZWJ で分割してそれぞれ <see cref="FindOther(ReadOnlySpan{char})"/> してみる。
        /// </summary>
        /// <returns><paramref name="indexes"/> に書き込んだ長さ。</returns>
        /// <remarks>
        /// さすがに <see cref="Find(ReadOnlySpan{char}, Span{int})"/> からの再起は要らないと思う。たぶん。
        /// <see cref="FindOther(ReadOnlySpan{char})"/> しか見ないので、国旗 + ZWJ とかは受け付けない。
        /// </remarks>
        private static int SplitZqjSequence(ZwjSplitResult zwjPositions, ReadOnlySpan<char> s, Span<int> indexes)
        {
            var totalWritten = 0;
            var prevPos = 0;

            for (int j = 0; j < ZwjSplitResult.MaxLength; j++)
            {
                var pos = zwjPositions[j];
                if (pos == 0) break;

                var written = ReduceExtends(s.Slice(prevPos, pos - prevPos), indexes);
                totalWritten += written;
                indexes = indexes.Slice(written);
                prevPos = pos + 1;
            }

            totalWritten += ReduceExtends(s.Slice(prevPos), indexes);

            return totalWritten;
        }


        /// <summary>
        /// Extend (FE0F と skin tone) 削り。
        /// FE0F → ただ消す。
        /// skin tone → 基本絵文字 + 肌色四角に分解。
        /// </summary>
        /// <returns><paramref name="indexes"/> に書き込んだ長さ。</returns>
        private static int ReduceExtends(ReadOnlySpan<char> s, Span<int> indexes)
        {
            // ZWJ 付きじゃないときに2度同じ処理してることになるけど、避けようと思うと結構大変なので妥協。
            // そんなに高頻度で来ないはずなので問題にもならないと思う。
            {
                var (_, written) = Find(s, indexes);
                return written;
            }

            // variation selector 16 削り。
            // FE0F (variation selector 16)は「絵文字扱いする」という意味なので、
            // RGI 的には FE0F なしで絵文字になってるものに余計に FE0F がくっついてても絵文字扱いしていい。
            if (s[s.Length - 1] == '\uFE0F')
            {
                // Find から再起するか(国旗 + FE0F とか、FE0F 複数個並べるとかに対応)までやるかどうか…
                var i = FindOther(s.Slice(0, s.Length - 1));

                var (_, written) = Find(s.Slice(0, s.Length - 1), indexes);
                return written;
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

        private static int FindOther(ReadOnlySpan<char> s, SkinTonePair skinTones = default)
        {
            var (singular, c) = GetSingularTable(s);

            if (singular is not null) return singular.TryGetValue(c, out var v) ? v : -1;

            if (skinTones.Length > 0) return FindeOtherWithSkinTone(s, skinTones);
            else return _otherTable.TryGetValue(s, out var v) ? v.index : -1;
        }

        private static int FindeOtherWithSkinTone(ReadOnlySpan<char> s, SkinTonePair tones)
        {
            Span<char> skinToneRemoved = stackalloc char[s.Length];
            int length = s.Length;

            if (tones.Length == 1)
            {
                if (char.IsSurrogate(s[0]))
                {
                    skinToneRemoved[0] = s[0];
                    skinToneRemoved[1] = s[1];
                    s.Slice(4).CopyTo(skinToneRemoved.Slice(2));
                    length -= 2;
                }
                else
                {
                    skinToneRemoved[0] = s[0];
                    s.Slice(3).CopyTo(skinToneRemoved.Slice(1));
                    length -= 2;
                }
            }
            else if (tones.Length == 2)
            {
                if (char.IsSurrogate(s[0]))
                {
                    skinToneRemoved[0] = s[0];
                    skinToneRemoved[1] = s[1];
                    s.Slice(4, s.Length - 6).CopyTo(skinToneRemoved.Slice(2));
                    length -= 4;
                }
                else
                {
                    skinToneRemoved[0] = s[0];
                    s.Slice(3, s.Length - 5).CopyTo(skinToneRemoved.Slice(1));
                    length -= 4;
                }
            }

            if (_otherTable.TryGetValue(skinToneRemoved.Slice(0, length), out var t))
            {
                var offset = OffsetFromSkinTone(t.skinVariationType, tones.Tone1, tones.Tone2);
                return t.index + offset;
            }

            // ちゃんとしたテーブルを作ってればここには来ないはずだけど一応。
            return _otherTable.TryGetValue(s, out var v) ? v.index : -1;
        }

        /// <summary>
        /// 1文字だけとか「1文字 + FE0F」の絵文字は特別扱いして char キーの辞書を作ってるので、そっちを引けるかの判定。
        /// </summary>
        private static (CharDictionary? singular, char c) GetSingularTable(ReadOnlySpan<char> s)
        {
            CharDictionary? singular = null;
            char c = '\0';

            if (s.Length == 1)
            {
                singular = _singularTable[0, 0];
                c = s[0];
            }
            else if (s.Length == 2)
            {
                if (s[1] == '\uFE0F')
                {
                    singular = _singularTable[1, 0];
                    c = s[0];
                }
                else
                {
                    if (s[0] == '\uD83C') singular = _singularTable[0, 1];
                    else if (s[0] == '\uD83D') singular = _singularTable[0, 2];
                    else if (s[0] == '\uD83E') singular = _singularTable[0, 3];
                    c = s[1];
                }
            }
            else if (s.Length == 3 && s[2] == '\uFE0F')
            {
                if (s[0] == '\uD83C') singular = _singularTable[1, 1];
                else if (s[0] == '\uD83D') singular = _singularTable[1, 2];
                else if (s[0] == '\uD83E') singular = _singularTable[1, 3];
                c = s[1];
            }

            return (singular, c);
        }

        /// <summary>
        /// emoji-data.json の並び的に、 skin_variations の並びは skin tone から機械的に決定できる。
        /// ただ、3パターンある。
        /// </summary>
        private static int OffsetFromSkinTone(byte type, SkinTone tone1, SkinTone tone2)
        {
            var t1 = (int)tone1;
            var t2 = (int)tone2;

            return type switch
            {
                // skin tone 1つ持ち
                1 => t1 + 1,
                // skin tone 2つ持ち(2人家族系)
                2 => 5 * t1 + t2 + 1,
                // 👫👬👭 用特殊処理
                3 => t1 == t2
                    ? t1 + 1
                    : 4 * t1 + t2 - (t1 < t2 ? 1 : 0) + 6,
                _ => 0, // 来ないはずだけど
            };
        }

        /// <summary>
        /// 絵文字シーケンス中含まれる skin tone を検索。
        /// </summary>
        /// <remarks>
        /// skin variation (skin tone 1F3FB-1F3FF で肌色変更)系の絵文字、RGI に入ってるやつは、
        /// - 1個目の skin tone は2文字目
        /// - 2個目の skin tone は末尾
        /// で固定。
        /// この前提で判定。
        ///
        /// この場合、skin tone を削った ZWJ sequence を検索してインデックスを取得した上で、
        /// skin tone から計算できるオフセットを足してインデックス計算できる。
        /// これをやれば skin tone の組み合わせ分(1個の絵文字シーケンス5倍、2個のやつなら25倍)テーブルデータ量を減らせるので頑張って計算することに。
        /// </remarks>
        private static (byte count, SkinTone tone1, SkinTone tone2) GetSkinTone(ReadOnlySpan<char> s)
        {
            SkinTone tone1;

            // 2文字目を調べる。
            if (char.IsHighSurrogate(s[0]))
            {
                // SMP + skin tone の場合も、
                if (s.Length < 4) return default;
                tone1 = GraphemeBreak.IsSkinTone(s.Slice(2));
                if (tone1 < 0) return default;
            }
            else
            {
                // BMP + skin tone の場合もある。
                if (s.Length < 3) return default;
                tone1 = GraphemeBreak.IsSkinTone(s.Slice(1));
                if (tone1 < 0) return default;
            }

            // tone1 が「2文字目、かつ、末尾」になってるときに tone2 と誤認しないように。
            if (s.Length < 5) return (1, tone1, 0);

            // 末尾調べる。
            var tone2 = GraphemeBreak.IsSkinTone(s.Slice(s.Length - 2));
            if (tone2 < 0) return (1, tone1, 0);
            return (2, tone1, tone2);
        }
    }
}
