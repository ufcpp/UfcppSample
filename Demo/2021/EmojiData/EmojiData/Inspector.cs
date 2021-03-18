using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EmojiData
{
    /// <summary>
    /// 適当に絵文字シーケンスの中身調査。
    /// </summary>
    class Inspector
    {
        /// <summary>
        /// 1符号点で1絵文字なやつ。
        /// </summary>
        public static bool IsSingleEmoji(Rune[] seq) => seq.Length == 1;

        /// <summary>
        /// 1符号点 + FE0F で1絵文字なやつ。
        /// </summary>
        public static bool IsVariationEquence(Rune[] seq) => seq.Length == 2 && seq[1].Value == 0xFE0F;

        /// <summary>
        /// 1符号点 + FE0F + 20E3 で1絵文字なやつ。
        /// keycaps しかないはず。
        /// </summary>
        public static bool IsKeycap(Rune[] seq) => seq.Length == 3 && seq[1].Value == 0xfe0f && seq[2].Value == 0x20E3;

        /// <summary>
        /// Region Indicator。
        /// 国旗(ISO 3166-1 alpha-2)で使うやつ。
        /// </summary>
        public static bool IsRegionIndicator(Rune r) => r.Value is >= 0x1F1E6 and <= 0x1F1FF;

        /// <summary>
        /// Region Indicator 2つで1絵文字なやつ。
        /// </summary>
        public static bool IsFlagSequence(Rune[] seq) => seq.Length == 2 && IsRegionIndicator(seq[0]) && IsRegionIndicator(seq[1]);

        /// <summary>
        /// Tag。
        /// 国旗(ISO_3166-2)で使うやつ。
        /// </summary>
        public static bool IsTag(Rune r) => r.Value is >= 0xE0020 and <= 0xE007F;

        /// <summary>
        /// スコットランド旗とかのえぐいやつ。
        /// </summary>
        public static bool IsTagSequence(Rune[] seq)
        {
            if (seq.Length == 1) return false;

            foreach (var r in seq.AsSpan(1))
            {
                if (!IsTag(r)) return false;
            }
            return true;
        }

        /// <summary>
        /// RGI に使われてる符号点の分布確認。
        /// </summary>
        public static void CountRunes(Rune[][] emojiSequenceList)
        {
            foreach (var g in emojiSequenceList
                .SelectMany(seq => seq)
                .Distinct()
                .GroupBy(r => r.Value switch
                    {
                        <= 0x80 => 0, // ascii, keycap 用の12文字(0-9 # *)のはず
                        <= 0x100 => 1, // latin1, copyright と registered の2文字のはず
                        <= 0x200d => 2, // ZWJ だけ浮いてるはず
                        <= 0x3300 => 3, // SJIS 由来とか、FE0F を付けた時だけ絵文字化するやつ、2000～3300 辺りにいるはず
                        <= 0x10000 => 4, // FE0F (異体字セレクター16)だけ浮いてるはず
                        <= 0x20000 => 5, // キャリア絵文字とかは 1F000 台に全部集まってるはず
                        _ => 6, // E0000 台の文字(subsivision flags 用のタグ文字)だけ残るはず
                    })
                .OrderBy(g => g.Key))
            {
                var count = 0;
                int min = int.MaxValue;
                int max = 0;

                foreach (var r in g)
                {
                    ++count;
                    min = Math.Min(min, r.Value);
                    max = Math.Max(max, r.Value);
                }

                Console.WriteLine($"{min:X}-{max:X} #{count}");
            }
        }

        /// <summary>
        /// keycaps は 0-9*# U+FE0F U+20E3 の3文字シーケンスのみ。
        /// かつ、ASCII 文字が混ざってるのはこの keycap の1文字目だけ。
        /// </summary>
        public static void Keycaps(Rune[][] emojiSequenceList)
        {
            var single = new List<Rune>();
            var fe0f = new List<Rune[]>(); // 2文字で、2文字目が U+FE0F。いわゆる variation sequence。
            var fe0f20e3 = new List<Rune[]>(); // 3文字で、2文字目が U+FE0F、3文字目が U+20E3。Keycap しかないはず。
            var other = new List<Rune[]>();

            foreach (var seq in emojiSequenceList)
            {
                if (IsSingleEmoji(seq))
                {
                    single.Add(seq[0]);
                }
                else if (IsVariationEquence(seq))
                {
                    fe0f.Add(seq);
                }
                else if (IsKeycap(seq))
                {
                    fe0f20e3.Add(seq);
                }
                else
                {
                    other.Add(seq);
                }
            }

            // Unicode 13.0 の ucd の方がちょっと数字大きいんだけど、emoji-data で未対応の文字あるのかな…
            Console.WriteLine("basic emoji (single code point): " + single.Count);
            Console.WriteLine("basic emoji (with variation selector): " + fe0f.Count);

            // 想定通り12文字
            Console.WriteLine("keycaps: " + fe0f20e3.Count);

            Console.WriteLine("keycap first char:");
            foreach (var keycaps in fe0f20e3)
            {
                Console.WriteLine("    " + keycaps[0]);
            }

            Console.WriteLine("except for keycaps...");

            var nonKeycapRunes = new HashSet<Rune>();
            foreach (var r in single) nonKeycapRunes.Add(r);
            foreach (var seq in fe0f.Concat(other))
            {
                foreach (var r in seq)
                {
                    nonKeycapRunes.Add(r);
                }
            }

            // 0
            Console.WriteLine("ascii count: " + nonKeycapRunes.Count(r => r.Value < 0x80));

            // copyright (U+00A9) と registered (U+00AE) の2文字だけのはず。
            Console.WriteLine("latin-1 count: " + nonKeycapRunes.Count(r => r.Value < 0x100));
        }

        /// <summary>
        /// 一応カテゴリー見てみたけど、符号点の範囲指定の方が楽そうだったのでほんとに一応見たというだけになったやつ。
        /// </summary>
        public static void Category(Rune[][] emojiSequenceList)
        {
            var list = emojiSequenceList
                // SJIS 由来の絵文字は OtherSymbol になってないのが多くて、
                // ↓この行の有無で残るカテゴリーの数がだいぶ変わる。
                .Where(seq => !IsSingleEmoji(seq) && !IsVariationEquence(seq) && !IsKeycap(seq))
                ;

            Console.WriteLine("## all categories:");

            foreach (var g in list.SelectMany(x => x).Distinct().GroupBy(r => Rune.GetUnicodeCategory(r)))
            {
                Console.WriteLine((g.Key, g.Count()));
            }

            Console.WriteLine("## first rune categories:");

            // 開始文字は OtherSymbol, ModifierSymbol, MathSymbol のみ。
            foreach (var g in list.Select(x => x[0]).Distinct().GroupBy(r => Rune.GetUnicodeCategory(r)))
            {
                Console.WriteLine((g.Key, g.Count()));

                if (g.Key == System.Globalization.UnicodeCategory.ModifierSymbol)
                {
                    // modifier symbols なのは skin tone の5文字だけ
                    Console.WriteLine("modifier symbols:");
                    foreach (var r in g)
                    {
                        Console.WriteLine("    " + r.Value.ToString("X"));
                    }
                }
            }
        }

        /// <summary>
        /// 「絵文字の分割だけなら https://unicode.org/reports/tr29/ ほど真面目に判定しなくていいよね」というのの確認のために、
        /// Extended_Pictographic (絵文字の1文字目に来るはずの符号点)と
        /// Extend (絵文字の2文字目に来るはずの符号点)の
        /// 大まかな値の範囲を調べる。
        /// </summary>
        /// <param name="emojiSequenceList"></param>
        public static void GraphemeBreak(Rune[][] emojiSequenceList)
        {
            // 先頭もしくは ZWJ 直後の文字。
            // UAX29 では emoji-data の Extended_Pictographic プロパティを調べろと言われてるもの。
            // 昔は E_Base (Emoji Modifier Base)ってプロパティだったので base にしとく。
            var seqBase = new HashSet<Rune>();

            // 上記以外。
            // UAX29 では GraphemeBreakProperty の Extend プロパティを調べろと言われてるもの。
            var seqExtend = new HashSet<Rune>();

            foreach (var x in emojiSequenceList)
            {
                // keycaps、国旗は変なので別判定するつもりで除外。
                if (IsKeycap(x) || IsFlagSequence(x) || IsTagSequence(x)) continue;

                var first = true;

                foreach (var r in x)
                {
                    if (r.Value == 0x200D) // zwj
                    {
                        first = true;
                        continue;
                    }

                    if (first)
                    {
                        first = false;
                        seqBase.Add(r);
                    }
                    else
                    {
                        seqExtend.Add(r);
                    }
                }
            }

            // BMP = Basic Multilingual Plane。サロゲートペアにならないやつ。16進数4桁以下。

            var minBmp = int.MaxValue;
            var maxBmp = 0;
            var countBmp = 0;
            foreach (var r in seqBase.Where(r => r.IsBmp)
                .Where(r => r.Value is not 0xA9 and not 0xAE) // keycaps、国旗を除くと浮いてるのは copyright と registered の2文字
                )
            {
                minBmp = Math.Min(minBmp, r.Value);
                maxBmp = Math.Max(maxBmp, r.Value);
                ++countBmp;
            }

            // 203C0～3299 の辺りになる。
            // ZWJ を避けるために > 0x200D and < 0x3300 とかで判定すればいいんじゃないかと。
            Console.WriteLine($"base rune range in BMP: {minBmp:X}-{maxBmp:X} #{countBmp}");

            // SMP = Supplementary Multilingual Plane。BMP はみ出た分。要はサロゲートペアになるやつ。

            var minSmp = int.MaxValue;
            var maxSmp = 0;
            var countSmp = 0;
            foreach (var r in seqBase.Where(r => !r.IsBmp))
            {
                minSmp = Math.Min(minSmp, r.Value);
                maxSmp = Math.Max(maxSmp, r.Value);
                ++countSmp;
            }

            // 全部 1F000 台。
            // UTF-16 なら high surrogate が D83C～D83F の間になるはずなのでこれで判定すればよさそう。
            Console.WriteLine($"base rune range in SMP: {minSmp:X}-{maxSmp:X} #{countSmp}");

            Console.WriteLine("extend rune:");
            foreach (var g in seqExtend.GroupBy(r => Rune.GetUnicodeCategory(r)))
            {
                Console.WriteLine((g.Key, g.Count()));

                // ModifierSymbol: skin tone の 1F3FB..1F3FF の5文字だけ
                // NonSpacingMark: FE0F (異体字セレクター16)の1文字だけ
                foreach (var r in g)
                {
                    Console.WriteLine("    " + r.Value.ToString("X"));
                }
            }
        }

        public static void Compare(Rune[][] emojiSequenceList, (Rune[] runes, int index, int variationType)[] unvariedEmojiSequenceList)
        {
            var unvariedIndex = 0;

            for (int index = 0; index < emojiSequenceList.Length;)
            {
                var emoji = emojiSequenceList[index];

                var unvaried = unvariedEmojiSequenceList[unvariedIndex];

                Debug.Assert(emoji.SequenceEqual(unvaried.runes));

                switch (unvaried.variationType)
                {
                    case 0:
                        ++index;
                        break;
                    case 1:
                        for (int i = 0; i < 5; i++)
                        {
                            var varied = emojiSequenceList[index + i + 1];
                            var skinToneRemoved = varied[2..].Prepend(varied[0]).ToArray();

                            // skin tone == i
                            Debug.Assert(emoji.SequenceEqual(skinToneRemoved));
                        }
                        index += 6;
                        break;
                    case 2:
                        for (int i = 0; i < 5; i++)
                        {
                            var varied = emojiSequenceList[index + i + 1];
                            Debug.Assert(emoji[1].Value == 0xFE0F);
                            var fe0fRemoved = emoji[2..].Prepend(emoji[0]).ToArray();
                            var skinToneRemoved = varied[2..].Prepend(varied[0]).ToArray();

                            // skin tone == i
                            Debug.Assert(fe0fRemoved.SequenceEqual(skinToneRemoved));
                        }
                        index += 6;
                        break;
                    case 3:
                        for (int i = 0; i < 25; i++)
                        {
                            var varied = emojiSequenceList[index + i + 1];
                            var skinToneRemoved = varied[2..^1].Prepend(varied[0]).ToArray();

                            // skin tone == i
                            Debug.Assert(emoji.SequenceEqual(skinToneRemoved));
                        }
                        index += 26;
                        break;
                    case 4:
                        for (int i = 0; i < 25; i++)
                        {
                            var varied = emojiSequenceList[index + i + 1];
                            Debug.Assert(emoji[1].Value == 0xFE0F);
                            var fe0fRemoved = emoji[2..].Prepend(emoji[0]).ToArray();
                            var skinToneRemoved = varied[2..^1].Prepend(varied[0]).ToArray();

                            // skin tone == i
                            Debug.Assert(fe0fRemoved.SequenceEqual(skinToneRemoved));
                        }
                        index += 26;
                        break;
                    default:
                        Debug.Assert(false, "来ないはず");
                        break;
                }

                if (emoji.Length == 1)
                {
                    if (emoji[0].Value is 0x1F46B or 0x1F46C or 0x1F46D)
                    {
                        ++unvariedIndex;
                        unvaried = unvariedEmojiSequenceList[unvariedIndex];

                        Debug.Assert(unvaried.variationType == 5);

                        for (int i = 0; i < 16; i++)
                        {
                            var varied = emojiSequenceList[index + i + 1];
                            var skinToneRemoved = varied[2..^1].Prepend(varied[0]).ToArray();

                            // skin tone == i
                            Debug.Assert(unvaried.runes.SequenceEqual(skinToneRemoved));
                        }
                        index += 20;
                    }
                }

                ++unvariedIndex;
            }
        }
    }
}
