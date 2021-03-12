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
        /// keycap が存在したらその文字列長を、
        /// なければ0を返す。
        ///
        /// 現状の仕様だともし見つかるなら3文字固定だし、
        /// 今後こんな変な仕様の絵文字を増やすとは思えないので、
        /// 実質的には0か3しか返さない。
        /// </returns>
        /// <remarks>
        /// 唯一 ASCII 文字開始の絵文字シーケンスでたちが悪いので先に判定。
        /// </remarks>
        public static int IsKeycap(ReadOnlySpan<char> s)
        {
            if (s.Length < 3) return 0;

            // combining enclosing keycap
            if (s[2] != 0x20E3) return 0;

            // variation selector-16
            if (s[1] != 0xFE0F) return 0;

            return s[0] switch
            {
                >= '0' and <= '9' => 3,
                '#' or '*' => 3,
                _ => 0,
            };
        }
    }
}
