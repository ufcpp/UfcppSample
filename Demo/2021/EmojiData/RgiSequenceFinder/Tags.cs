using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RgiSequenceFinder
{
    /// <summary>
    /// Emoji tag sequence のタグ部分を相当する ASCII 文字列化したもの。
    /// 参考: http://unicode.org/reports/tr51/#def_std_emoji_tag_sequence_set
    /// </summary>
    /// <remarks>
    /// 1F3F4-E0067-E0062-E0073-E0063-E0074-E007F であれば gbsct [ESC] の6文字。
    ///
    /// タグ数が <see cref="TagMaxLength"/> 以上のときは情報が切り捨てられるので注意
    /// (<see cref="GraphemeBreak"/> 側で <see cref="EmojiSequenceType.MoreBufferRequired"/> 化してる)。
    ///
    /// 逆にタグ数が  <see cref="TagMaxLength"/> より少ない時(というか、現状6しかあり得ない)、後ろの方(現状、末尾2文字)は0詰め。
    ///
    /// 先頭の文字(現状の RGI では 🏴 (1F3F4) 以外ありえない)は今、単に削除しちゃってる。
    /// 先頭文字を残すかどうかは後々というか、実際のところあり得ないとは思うけども、旗以外の emoji tag sequence が追加されたらまた改めて考える。
    /// </remarks>
    public struct Tags
    {
        // 現状、emoji tag sequence のタグが6文字以上の RGI 絵文字はないんだけど、
        // どうせ alignment で8に揃えられたりするので8バイト取っとく。
        public byte Tag0;
        public byte Tag1;
        public byte Tag2;
        public byte Tag3;
        public byte Tag4;
        public byte Tag5;
        public byte Tag6;
        public byte Tag7;

        public const int TagMaxLength = 8;

        public static readonly Tags TooLong = new() { Tag0 = 255 };

        /// <summary>
        /// 🏴 始まりの emoji tag sequence かどうかを判定。
        /// そうなってないときは count == 0 を返す。
        /// </summary>
        /// <remarks>
        /// 絵文字の闇その2。
        ///
        /// <see cref="IsFlag(ReadOnlySpan{char})"/> の仕様では表せない地域からクレーム入ったという話。
        /// ISO 3166-1 (ラテン文字2文字で表現する国コード)で表されてる「国または地域」の旗だけ用意したらそりゃ怒られるよね。
        /// ISO 3166-2 (ラテン文字4～5文字で表現する行政区画コード)で旗を出す。
        ///
        /// 🏴 (U+1F3F4、黒旗、UTF-16 で D83C DFF4)の後ろに「タグ文字」っていう特殊な記号を並べる仕様。
        /// タグ文字は E0000 の辺りに並んでる。
        /// 通常、0-9、a-z 相当の36文字 + cancel 文字(E007F)しか使わないと思うけど、
        /// 一応、E0000-E007F で判定しとく。
        /// high surrogate が DB40、
        /// low surrogate が DC00-DC7F。
        ///
        /// 将来他の用途でも使うかもしれないので「flag sequence」じゃなくて「tag sequence」になってる。
        /// 一応原理上は「東京都の旗」(🏴JP13)とかも出せる。
        /// RGI に入ってるのはグレートブリテン島の3カントリーだけ(というかこのカントリー旗のためにこの仕様が入った)。
        ///
        /// タグ文字を使う仕様がこいつだけなので、これも先に判定してしまえば他の絵文字シーケンス処理から E0000 台の判定を消せる。
        /// </remarks>
        public static (int count, Tags tags) FromFlagSequence(ReadOnlySpan<char> s)
        {
            if (s.Length < 2) return default;

            if (s[0] != 0xD83C) return default;
            if (s[1] != 0xDFF4) return default;

            var i = 0;
            Tags tags = default;
            var tagsSpan = tags.AsSpan();

            s = s.Slice(2);

            while (s.Length >= 2)
            {
                if (s[0] != 0xDB40) break;
                if (!isTagLowSurrogate(s[1])) break;

                if (i <= 8)
                {
                    tagsSpan[i] = (byte)(s[1] - 0xDC00);
                }
                ++i;
                s = s.Slice(2);
            }

            // Tag が付いてないときは 0 にしないと 1F3F4-200D-2620-FE0F (海賊旗)とかを拾っちゃう。
            return (i, tags);

            static bool isTagLowSurrogate(char c) => c is >= (char)0xDC00 and <= (char)0xDC7F;
        }

        public override string ToString()
        {
            if (Tag0 == 0) return "NONE";
            if (Tag0 == 255) return "TOO LONG";

            var sb = new StringBuilder();
            var span = this.AsSpan();

            foreach (var c in span)
            {
                if (c is 0x7f or 0) break;
                sb.Append((char)c);
            }
            return sb.ToString();
        }
    }

    internal static class TagSequenceExtensions
    {
        public static Span<byte> AsSpan(ref this Tags tags) => MemoryMarshal.CreateSpan(ref tags.Tag0, 8);
    }
}
