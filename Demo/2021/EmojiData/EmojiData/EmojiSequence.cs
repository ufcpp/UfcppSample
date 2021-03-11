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
    }
}
