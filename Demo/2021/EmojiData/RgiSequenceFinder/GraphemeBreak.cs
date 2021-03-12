using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgiSequenceFinder
{
    /// <summary>
    /// 絵文字がらみ、結局最終的にはテーブルを引く以外の手段がないものの、
    /// 「将来、絵文字シーケンスとして追加するかもしれないものの判定のために、まず UAX #29 にそって grapheme cluster 分割してくれ」と言うことになってる。
    /// 参考: https://unicode.org/reports/tr29/
    ///
    /// これ、絵文字に限定すればここまで複雑な処理必要ないはずなので、簡易版みたいな判定をする。
    ///
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
        /// 国旗を判定。
        /// 参考: http://unicode.org/reports/tr51/#def_std_emoji_flag_sequence_set
        /// </summary>
        /// <returns>
        /// 国旗絵文字が存在したらその文字列長を、
        /// なければ0を返す。
        ///
        /// これも2文字固定の仕様で、これは今後多分 Unicode 的にも変更不可能なレベルの仕様だと思うので、
        /// <see cref="IsKeycap(ReadOnlySpan{char})"/> 以上の確度で0か2しか返さない。
        /// </returns>
        /// <remarks>
        /// キャリア絵文字の闇その1。
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
        public static int IsFlag(ReadOnlySpan<char> s)
        {
            if (s.Length < 4) return 0;

            if (s[0] != 0xD83C || s[2] != 0xD83C) return 0;

            if (!isRegionalIndicatorLowSurrogate(s[1])) return 0;
            if (!isRegionalIndicatorLowSurrogate(s[3])) return 0;

            return 2;

            static bool isRegionalIndicatorLowSurrogate(char c) => c is >= (char)0xDDE6 and <= (char)0xDDFF;
        }
    }
}
