using System;

namespace RgiSequenceFinder
{
    /// <summary>
    /// 絵文字がらみ、結局最終的にはテーブルを引く以外の手段がないものの、
    /// 「将来、絵文字シーケンスとして追加するかもしれないものの判定のために、まず UAX #29 にそって grapheme cluster 分割してくれ」と言うことになってる。
    /// 参考: https://unicode.org/reports/tr29/
    ///
    /// これ、絵文字に限定すればここまで複雑な処理必要ないはずなので、簡易版みたいな判定をする。
    ///
    /// 文字数を返すメソッドが多くなるけど、全部 UTF-16 code unit 数(C# の string.Length)。
    /// (UTF-8 byte 数、rune 数、grapheme 数ではない。)
    /// </summary>
    /// <remarks>
    /// ちなみに、 .NET 5 以降なら <see cref="System.Globalization.StringInfo.GetTextElementEnumerator(string)"/> がちゃんと grapheme cluster 分割してくれる。
    /// (.NET Core 3.1 までは絵文字に対応してなかった。)
    ///
    /// Unicode 10.0 の時に grapheme cluster 分割のロジックを C# で書いたこともあり。
    /// https://github.com/ufcpp/GraphemeSplitter
    /// これ、prepend (サンスクリットで使う)だけ対応してなかったり。
    /// あと、<see cref="System.Globalization.StringInfo"/> 公式対応が入ったので今後保守するつもりはない。
    /// </remarks>
    public class GraphemeBreak
    {
        /// <summary>
        /// <see cref="System.Globalization.StringInfo.GetNextTextElementLength"/> 的なことを絵文字シーケンス専用にしたもの。
        /// </summary>
        /// <returns>
        /// Emoji sequence (単独の絵文字含む)判定を受ける場合、その長さ(UTF-16 長)を返す。
        /// それ以外の文字(絵文字表示のロジック的には無視して素通しする文字)なら0。
        /// </returns>
        /// <remarks>
        /// 正規表現的に書くと
        /// Extended_Pictographic (Extend* ZWJ Extended_Pictographic)* Extend*
        /// な判定。
        ///
        /// <see cref="IsPictgraphicEstimate(Span{char})"/> と <see cref="IsExtendEstimate(Span{char})"/> のコメントも参照。
        /// Estimate (大体の予測)って名前が付いているのからお察しな通り、本来の grapheme cluster 分割よりもだいぶ荒い。
        /// 「この後 RGI 絵文字シーケンスの判定はどの道テーブルを引くしかないから正確な判定はそっちできるはず」という前提。
        /// </remarks>
        public static (EmojiSequenceType type, int length) GetEmojiSequenceLength(ReadOnlySpan<char> s)
        {
            // パフォーマンス用。 empty 時に early return。
            if (s.Length == 0) return (EmojiSequenceType.NotEmoji, 0);

            if (IsKeycap(s)) return (EmojiSequenceType.Keycap, 3);

            // パフォーマンス用。keycap 以外に ASCII 出てこないので ASCII 用 fast path。
            if (s[0] < 0x80) return (EmojiSequenceType.NotEmoji, 1);

            // ここから下の判定に入らないこと荒く判定できるんで、それで先にはじいちゃう fast path。
            if (!CanBePictgraphic(s[0])) return (EmojiSequenceType.NotEmoji, 1);

            // RI 国旗。
            if (IsFlagSequence(s) is { First: not 0 }) return (EmojiSequenceType.Flag, 4);

            // Tag 国旗。
            var (tagCount, _) = Tags.FromFlagSequence(s);
            if (tagCount != 0)
            {
                var type = tagCount > Tags.TagMaxLength ? EmojiSequenceType.MoreBufferRequired : EmojiSequenceType.Tag;
                return (type, 2 * tagCount + 2);
            }

            var count = IsZwjSequence(s);

            if (count == 0) return (EmojiSequenceType.NotEmoji, 1);
            else return (EmojiSequenceType.Other, count);
        }

        /// <summary>
        /// RGI emoji keycap sequence を判定。
        /// 参考: https://unicode.org/reports/tr51/#def_std_emoji_keycap_sequence_set
        /// </summary>
        /// <param name="s">判定対象</param>
        /// <returns>
        /// keycap だったら true。
        ///
        /// keycap みたいな変な仕様、今後追加されるとは思えないので「3文字固定」だと思って扱うことにする。
        /// </returns>
        /// <remarks>
        /// 唯一 ASCII 文字開始の絵文字シーケンスでたちが悪いので先に判定。
        /// </remarks>
        public static bool IsKeycap(ReadOnlySpan<char> s)
        {
            if (s.Length < 3) return false;

            // combining enclosing keycap
            if (s[2] != 0x20E3) return false;

            // variation selector-16
            if (s[1] != 0xFE0F) return false;

            return s[0] is (>= '0' and <= '9') or '#' or '*';
        }

        /// <summary>
        /// Emoji flag sequence (RGI に限らない)を判定。
        /// 参考: http://unicode.org/reports/tr51/#def_std_emoji_flag_sequence_set
        /// </summary>
        /// <returns>
        /// 国旗絵文字が存在したら国コードに対応する数値(<see cref="RegionalIndicator"/>)を、
        /// なければ-1を返す。
        ///
        /// これも2文字固定(UTF-16 だと4文字固定)なので、文字列長は返さなくていいはず。
        /// (仕様変更は不可能なレベルだと思うので将来固定長でなくなる心配は多分要らない。)
        /// 戻り値を int とか short にしちゃうと他のメソッドの「grapheme cluster 長を返す」って仕様と混ざるのが怖いので専用の型を作った。
        /// </returns>
        /// <remarks>
        /// 絵文字の闇その1。
        ///
        /// 他の grapheme 判定と違って「特定の範囲の文字が2文字並んでいるときに区切る」っていう特殊仕様。
        /// (他は正規表現でいうところの * (0個以上)判定)。
        ///
        /// 他の仕様と完全に独立だし、「必ず2文字で区切る」って処理がかなり変なのでこれも先に判定。
        ///
        /// Regional Indicator っていう 1F1E6-1F1FF の26文字を使う。
        /// UTF-16 の場合、
        /// high surrogate が D83C 固定で、
        /// low surrogate が DDE6-DDFF。
        /// </remarks>
        public static RegionalIndicator IsFlagSequence(ReadOnlySpan<char> s)
        {
            if (s.Length < 4) return RegionalIndicator.Invalid;

            if (s[0] != 0xD83C || s[2] != 0xD83C) return RegionalIndicator.Invalid;

            if (!isRegionalIndicatorLowSurrogate(s[1])) return RegionalIndicator.Invalid;
            if (!isRegionalIndicatorLowSurrogate(s[3])) return RegionalIndicator.Invalid;

            return new(s[1], s[3]);

            static bool isRegionalIndicatorLowSurrogate(char c) => c is >= (char)0xDDE6 and <= (char)0xDDFF;
        }

        /// <summary>
        /// UAX #29 の Extended_Pictographic をちょっと緩めに判定。
        /// </summary>
        /// <returns>
        /// 長さ(rune 単位で判定するものの、UTF-16 なので1か2があり得る)。
        /// Extended_Pictographic 判定を受けなかったものは0。
        /// </returns>
        /// <remarks>
        /// この後どうせ、RGI かどうかの判定はテーブル引きするので、
        /// Extended_Pictographic かどうかの判定はかなり「大は小を兼ねる」発想でやる。
        ///
        /// © (00A9、copyright)と ® (00AE、registered)だけかなり浮いた位置にいるのでこの2つだけ個別判定。
        /// 残りは 200E～32FF と 1F000 台全部 Extended_Pictographic 扱い。
        /// </remarks>
        public static int IsPictgraphicEstimate(ReadOnlySpan<char> s)
        {
            if (s.Length >= 2)
            {
                if ((s[0] | 0b11) == 0xD83F && char.IsLowSurrogate(s[1]))
                {
                    return 2;
                }
            }
            if (s.Length >= 1)
            {
                if (CanBePictgraphicBmp(s[0]))
                {
                    return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// <see cref="GetEmojiSequenceLength(ReadOnlySpan{char})"/> の ZWJ シーケンス判定ループに入る前に1文字限り見て early return するためのさらに荒い判定。
        /// 「サロゲートペアだから1文字後ろを読んで判定」みたいな処理が重たいので、サロゲートペア素通しな判定を1回やる。
        /// </summary>
        private static bool CanBePictgraphic(char c) => CanBePictgraphicBmp(c) || char.IsHighSurrogate(c);

        /// <summary>
        /// BMP 内で Extended_Pictographic 候補になる文字。
        /// </summary>
        private static bool CanBePictgraphicBmp(char c) => c is (> (char)0x200D and < (char)0x3300) or '©' or '®';

        /// <summary>
        /// UAX #29 の Extend を絵文字専用に判定。
        /// </summary>
        /// <returns>
        /// 長さ(rune 単位で判定するものの、UTF-16 なので1か2があり得る)。
        /// Extend 判定を受けなかったものは0。
        /// </returns>
        /// <remarks>
        /// Extend の大半は U+0301 の ́  みたいに「他の文字にくっつけて表示する0幅文字」みたいなの。
        /// 絵文字と組み合わせて表示できるレンダリング システムほとんどないと思うし、もちろん RGI にそんな文字含まれない。
        ///
        /// 絵文字相手に使う Extend は実際のところ以下の6文字だけ。
        ///
        /// FE0F: 異体字セレクター16 (variation selector-16)
        /// 1F3FB-1F3FF: 肌色選択修飾子(emoji modifier Fitzpatrick)
        ///
        /// 後者は UTF-16 だと
        /// high surrogate が D83C、
        /// low surrogate が DFFB-DFFF。
        ///
        /// Fitzpatrick skin type も絵文字の闇。
        /// こいつだけ「他の絵文字の後ろにくっつく」特殊仕様。
        /// 後々の追加された絵文字シーケンスの場合は必ず前の絵文字との間に ZWJ を挟む仕様になってる。
        /// 男女の選択は ZWJ + ♂、♀ だし、髪型選択は ZWJ + 1F9B0-1F9B3。
        /// </remarks>
        public static int IsExtendEstimate(ReadOnlySpan<char> s)
        {
            if (s.Length >= 2)
            {
                if (s[0] == 0xD83C && isFitzpatrickLowSurrogate(s[1]))
                {
                    return 2;
                }
            }
            if (s.Length >= 1)
            {
                if (s[0] == 0xFE0F)
                {
                    return 1;
                }
            }
            return 0;

            static bool isFitzpatrickLowSurrogate(char c) => c is >= (char)0xDFFB and <= (char)0xDFFF;
        }

        /// <summary>
        /// <see cref="GetEmojiSequenceLength(ReadOnlySpan{char})"/> の主要処理。
        /// 「keycap と国旗を除けばだいぶシンプルになる」前提の Emoji ZWJ sequence 判定。
        /// </summary>
        private static int IsZwjSequence(ReadOnlySpan<char> s)
        {
            // ZWJ シーケンス。
            var count = 0;
            while (true)
            {
                var pict = IsPictgraphicEstimate(s);

                if (pict == 0) break;

                count += pict;
                s = s.Slice(pict);

                while (true)
                {
                    var ext = IsExtendEstimate(s);

                    if (ext == 0) break;

                    count += ext;
                    s = s.Slice(ext);
                }

                if (s.Length >= 1 && s[0] == (char)0x200D)
                {
                    ++count;
                    s = s.Slice(1);
                }
                else break;
            }

            return count;
        }
    }
}
