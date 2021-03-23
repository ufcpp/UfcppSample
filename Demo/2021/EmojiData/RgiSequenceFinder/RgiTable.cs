using System;

namespace RgiSequenceFinder
{
    internal partial class RgiTable
    {
        /// <summary>
        /// <see cref="Finder.Find(ReadOnlySpan{char}, Span{EmojiIndex})"/>
        ///
        /// <see cref="RgiTable"/> 自体を public にするのはいまいちかもと思って <see cref="Finder"/> をあとから分けた名残り。
        /// </summary>
        public static (int charRead, int indexWritten) Find(ReadOnlySpan<char> s, Span<EmojiIndex> indexes)
        {
            // 以下のコード、 Length チェックなしで indexex[0] を書いちゃってるんで、
            // 最初に if (Length == 0) return した方がいいかも。

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

                        // ほんとは正確な仕様ではないものの、未対応の Flag sequence は ASCII 2文字に展開しちゃう。
                        //
                        // Unicode 的には「Regional Indicator の片割れは絵文字候補じゃない」扱いだし表示しなくていいと思う。
                        // が、ポリティカルな理由で意図的に国旗を表示しない某 OS は全ての Flag sequence をアルファベット2文字で表示してるし、
                        // たいていのプラットフォームは RI の片割れを「四角囲みのアルファベット」の絵で表示してる。
                        // 四角囲みのアルファベットは emoji-data.json にデータがないので、うちは ASCII 2文字に変換してしまうことに。

                        if (i < 0)
                        {
                            indexes[0] = new((char)emoji.Region.First);
                            if (indexes.Length > 1) indexes[1] = new((char)emoji.Region.Second);
                            return (4, 2);
                        }

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
        private static int SplitZqjSequence(ZwjSplitResult zwjPositions, ReadOnlySpan<char> s, Span<EmojiIndex> indexes)
        {
            var totalWritten = 0;
            var prevPos = 0;

            // ZWJ なしの単体。
            if(zwjPositions[0] == 0)
            {
                return ReduceExtends(s, indexes, false);
            }

            for (int j = 0; j < ZwjSplitResult.MaxLength; j++)
            {
                var pos = zwjPositions[j];
                if (pos == 0) break;

                var written = ReduceExtends(s.Slice(prevPos, pos - prevPos), indexes, true);
                totalWritten += written;
                indexes = indexes.Slice(written);
                prevPos = pos + 1;
            }

            totalWritten += ReduceExtends(s.Slice(prevPos), indexes, true);

            return totalWritten;
        }


        /// <summary>
        /// Extend (FE0F と skin tone) 削り。
        /// FE0F → ただ消す。
        /// skin tone → 基本絵文字 + 肌色四角に分解。
        /// </summary>
        /// <returns><paramref name="indexes"/> に書き込んだ長さ。</returns>
        private static int ReduceExtends(ReadOnlySpan<char> s, Span<EmojiIndex> indexes, bool allowCharFallback)
        {
            if (s.Length == 0) return 0;

            var firstChar = char.IsHighSurrogate(s[0]) ? 2 : 1;

            // variation selector 16 削り。
            // FE0F (variation selector 16)は「絵文字扱いする」という意味なので、
            // RGI 的には FE0F なしで絵文字になってるものに余計に FE0F がくっついてても絵文字扱いしていい。
            if (s.Length >= firstChar + 1 && s[s.Length - 1] == '\uFE0F')
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

            if (s.Length >= firstChar + 2)
            {
                var st = GraphemeBreak.IsSkinTone(s.Slice(firstChar));
                if (st >= 0)
                {
                    // ZWJ 分割後に RGI になってる部分があるので再検索。
                    // 最初にやった「ZWJ 分割のついでに skin tone 記録」も使えないので作り直す。
                    var i = FindOther(s.Slice(0, firstChar + 2), new(st, SkinTone.None));

                    if (i >= 0)
                    {
                        if (indexes.Length > 0) indexes[0] = i;
                        return 1;
                    }

                    // なければ元の絵 + 肌色四角を返す。
                    i = FindOther(s.Slice(0, firstChar));

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

            // ZWJ 分割後が普通に skin tone も FE0F も付いてない絵文字なことは多々あるので再検索。
            {
                var i = FindOther(s);

                if (i >= 0)
                {
                    if (indexes.Length > 0) indexes[0] = i;
                    return 1;
                }
            }

            if (allowCharFallback)
            {
                // ZWJ sequence のときだけ文字素通ししたいので分岐。
                indexes[0] = GetChar(s);
                return 1;
            }
            else return 0;
        }

        /// <summary>
        /// 文字素通し <see cref="EmojiIndex"/> を返す。
        /// </summary>
        /// <remarks>
        /// ZWJ 分割後は非絵文字が混ざることがある。
        /// 一部の「単独だと絵文字扱いをうけないけども FE0F が付いてれば絵文字」な類の文字が、
        /// ZWJ Sequence 内では単独で出てきたりする。
        /// その場合、その文字を単体で返す。
        /// </remarks>
        private static EmojiIndex GetChar(ReadOnlySpan<char> s)
        {
            if (char.IsHighSurrogate(s[0]))
            {
                // 不正な UTF-16 の時どうしよう。例外の方がいい？
                if (s.Length < 2 || !char.IsLowSurrogate(s[1]))
                    return new('\0');

                return new(s[0], s[1]);
            }
            else
            {
                return new(s[0]);
            }
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

            var toneLength = tones.Length;

            if (toneLength > 0)
            {
                var firstChar = char.IsHighSurrogate(s[0]) ? 2 : 1; // UTF-16 なので、「2文字目」と言いつつサロゲートペアで個数分岐。
                var lastRemoveChar = toneLength == 1 ? 0 : 2; // skin tone 2つの時は末尾を UTF-16 2個分削る。

                s.Slice(0, firstChar).CopyTo(skinToneRemoved);
                s.Slice(2 + firstChar, s.Length - 2 - firstChar - lastRemoveChar).CopyTo(skinToneRemoved.Slice(firstChar));
                length -= 2 + lastRemoveChar;
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
    }
}
