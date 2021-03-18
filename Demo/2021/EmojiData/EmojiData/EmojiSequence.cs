using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace EmojiData
{
    /// <summary>
    /// emoji.json の unified プロパティを拾えば、RGI 絵文字シーケンスの一覧が取れるはず。
    /// たぶん、emoji-data の大本をさらにたどると
    /// https://www.unicode.org/Public/emoji/13.1/emoji-sequences.txt
    /// https://www.unicode.org/Public/emoji/13.1/emoji-zwj-sequences.txt
    /// にたどり着くはずで、バージョンさえ合わせればここからデータを取るのと、順序を除いて、多分同じ結果になると思う。
    /// </summary>
    class EmojiSequence
    {
        /// <summary>
        /// 1F408-200D-2B1B みたいなハイフン区切りの16進数が並んでるはずなのでその前提で Rune 化。
        /// a～f の大文字・小文字は混在してる。
        /// </summary>
        static IEnumerable<Rune> Parse(string hyphenatedCodePoints)
        {
            var cp = 0;
            foreach (var c in hyphenatedCodePoints)
            {
                if (c is >= '0' and <= '9') cp = cp * 16 + (c - '0');
                else if (c is >= 'a' and <= 'f') cp = cp * 16 + (c - 'a' + 10);
                else if (c is >= 'A' and <= 'F') cp = cp * 16 + (c - 'A' + 10);
                else if (c is '-')
                {
                    yield return new(cp);
                    cp = 0;
                }
            }
            yield return new(cp);
        }

        /// <summary>
        /// emoji.json の unified 行を読んで <see cref="Rune"/> 配列化したものを列挙。
        /// </summary>
        public static IEnumerable<Rune[]> EnumerateRgiEmojiSequence(JsonDocument doc)
        {
            foreach (var elem in doc.RootElement.EnumerateArray())
            {
                yield return parseUnified(elem);

                if (elem.TryGetProperty("skin_variations", out var skinVariations))
                {
                    foreach (var variation in skinVariations.EnumerateObject())
                    {
                        yield return parseUnified(variation.Value);
                    }
                }
            }

            string readUnified(JsonElement elem)
            {
                if (elem.TryGetProperty("unified", out var unified) && unified.GetString() is { } us) return us;
                throw new KeyNotFoundException();
            }

            Rune[] parseUnified(JsonElement elem)
            {
                return Parse(readUnified(elem)).ToArray();
            }
        }

        private static readonly Rune[] replace1F46B = new[] { 0x1F469, 0x200D, 0x1F91D, 0x200D, 0x1F468 }.Select(x => new Rune(x)).ToArray();
        private static readonly Rune[] replace1F46C = new[] { 0x1F468, 0x200D, 0x1F91D, 0x200D, 0x1F468 }.Select(x => new Rune(x)).ToArray();
        private static readonly Rune[] replace1F46D = new[] { 0x1F469, 0x200D, 0x1F91D, 0x200D, 0x1F469 }.Select(x => new Rune(x)).ToArray();

        /// <summary>
        /// emoji.json の unified 行を読んで <see cref="Rune"/> 配列化したものを列挙。
        /// <see cref="EnumerateRgiEmojiSequence(JsonDocument)"/> は skin tone バリエーションも平坦化して列挙したけど、
        /// こっちはバリエーションの列挙はなし。
        /// その代わり、バリエーションの種類を返す。
        /// (種類だけわかれば、元の文字と skin tone からインデックスを機械的に計算可能。)
        ///
        /// 0: バリエーションなし
        /// 1: skin tone 1個
        /// 2: skin tone 2個
        /// 3: 👫👬👭 skin tone 2個なんだけど、バリエーションの持ち方が特殊
        ///
        /// 👫👬👭 は、それ自体は 2 のパターン。
        /// それとは別に、
        ///
        /// 1F46B → 1F469-200D-1F91D-200D-1F468
        /// 1F46C → 1F468-200D-1F91D-200D-1F468
        /// 1F46D → 1F469-200D-1F91D-200D-1F469
        ///
        /// に置き換えた絵文字を RGI と同列に扱った上で、これ専用のインデックス計算が必要。
        /// </summary>
        public static IEnumerable<(Rune[] runes, int index, int variationType)> EnumerateUnvariedRgiEmojiSequence(JsonDocument doc)
        {
            var index = 0;
            foreach (var elem in doc.RootElement.EnumerateArray())
            {
                var runes = parseUnified(elem);

                if (runes.Length == 1)
                {
                    if (runes[0].Value == 0x1F46B)
                    {
                        yield return (runes, index, 1);
                        yield return (replace1F46B, index, 3);
                        index += 26;
                        continue;
                    }
                    if (runes[0].Value == 0x1F46C)
                    {
                        yield return (runes, index, 1);
                        yield return (replace1F46C, index, 3);
                        index += 26;
                        continue;
                    }
                    if (runes[0].Value == 0x1F46D)
                    {
                        yield return (runes, index, 1);
                        yield return (replace1F46D, index, 3);
                        index += 26;
                        continue;
                    }
                }

                if (!elem.TryGetProperty("skin_variations", out var skinVariations))
                {
                    yield return (runes, index, 0);
                    ++index;
                    continue;
                }

                var count = skinVariations.EnumerateObject().Count();
                if (count == 5)
                {
                    if (runes.Length >= 2 && runes[1].Value == 0xFE0F)
                    {
                        yield return (runes, index, 1);
                        yield return (runes[2..].Prepend(runes[0]).ToArray(), index, 1); // 2文字目の FE0F を削った物も出力しておく。
                    }
                    else
                    {
                        yield return (runes, index, 1);
                    }
                    index += 6;
                }
                else if (count == 25)
                {
                    if (runes.Length >= 2 && runes[1].Value == 0xFE0F)
                    {
                        yield return (runes, index, 2);
                        yield return (runes[2..].Prepend(runes[0]).ToArray(), index, 2); // 2文字目の FE0F を削った物も出力しておく。
                    }
                    else
                    {
                        yield return (runes, index, 2);
                    }
                    index += 26;
                }
                else
                {
                    throw new System.Exception("来ないはず");
                }
            }

            string readUnified(JsonElement elem)
            {
                if (elem.TryGetProperty("unified", out var unified) && unified.GetString() is { } us) return us;
                throw new KeyNotFoundException();
            }

            Rune[] parseUnified(JsonElement elem)
            {
                return Parse(readUnified(elem)).ToArray();
            }
        }
    }
}
