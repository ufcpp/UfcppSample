using System;

namespace RgiSequenceFinder
{
    public static class Finder
    {
        /// <summary>
        /// <paramref name="source"/> に大して <see cref="Find(ReadOnlySpan{char}, Span{EmojiIndex})"/> を書けて、
        /// <see cref="EmojiIndex.IsIndex"/> のときは U+E000 + index の文字に置き換える。
        /// </summary>
        /// <remarks>
        /// U+E000～ の6400文字は外字領域(private use area)。
        /// 最初は「どの文字に置き換えるか」はカスタマイズ可能に作ろうかとも思ったけども、
        /// 大して需要がない割に実行時性能落としそうだったのでもう固定で行くことに。
        ///
        /// せめてクラスを分けるか、インスタンスメソッドにして「開始文字」をフィールドに持つかは考えたけど、
        /// それも「需要があれば別途実装」で。
        ///
        /// Unicode 13.0 時点で RGI 絵文字シーケンスは3300文字なので、6400文字もあれば十分かなと言うことで U+E000～ を利用。
        /// U+F0000～10FFFF の2面 約13万文字も private use area らしいけども、サロゲートペアの書き込み負担もバカにならないのでこっちを使うのはやめた。
        /// </remarks>
        /// <param name="source">元の文字列。</param>
        /// <param name="destination">
        /// 置換結果の書き込み先。
        /// たぶん、現実装だと<paramref name="source"/>の<see cref="string.Length"/>を超えることないはず。
        /// </param>
        /// <returns>
        /// <paramref name="destination"/> に書き込んだ文字列長。
        /// ただし、<paramref name="source"/> と1文字も変更がなかった場合は0を返す。
        /// </returns>
        public static int Replace(ReadOnlySpan<char> source, Span<char> destination)
        {
            const char StartChar = '\uE000'; // private use area の先頭文字。

            // 現実的にはたいてい最大でも2だし、レアなやつでも4くらいだと思うけど、一応16要素バッファー持っとく。
            // 👩‍👩‍👧‍👧‍👧‍👧‍👧‍👧‍👧‍👧‍👧‍👧‍👧‍👧 
            // ↑こういうマネ(家族絵文字の後ろに延々と ZWJ + boy or girl 連結)すると Windows だと1つの絵文字にくっついたりするんだけど、
            // まあ、わざわざそんな絵文字を打つ人もいないと思うし。
            Span<EmojiIndex> indexes = stackalloc EmojiIndex[16];

            int totalWritten = 0;
            bool any = false;
            while (true)
            {
                var (read, written) = Find(source, indexes);
                var charWritten = 0;

                if (written == 0)
                {
                    // 絵文字がなかった → 元の文字素通し。
                    for (int i = 0; i < read; i++)
                    {
                        destination[i] = source[i];
                    }
                    charWritten = read;
                }
                else
                {
                    any = true;
                    // インデックスを外字領域文字に置き換え。
                    for (int i = 0; i < written; i++)
                    {
                        var index = indexes[i];

                        if (index.IsIndex)
                        {
                            destination[charWritten] = (char)(StartChar + index.Index);
                            ++charWritten;
                        }
                        else
                        {
                            charWritten += index.WriteUtf16(destination);
                        }
                    }
                }

                totalWritten += charWritten;
                source = source.Slice(read);
                destination = destination.Slice(charWritten);

                if (source.Length == 0) break;
            }

            // 結局、全文字素通しだった場合は0を返しとく。 new string(buffer, 0, len) のアロケーション除けのため。
            if (!any) return 0;

            return totalWritten;
        }

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
        public static (int charRead, int indexWritten) Find(ReadOnlySpan<char> source, Span<EmojiIndex> indexes)
            => RgiTable.Find(source, indexes);
    }
}
