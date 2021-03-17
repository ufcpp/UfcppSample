using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EmojiData
{
    /// <summary>
    /// emoji.json の JSON データ構造確認。
    /// </summary>
    class JsonDocChecker
    {
        public static void Check(JsonDocument doc)
        {
            Debug.Assert(doc.RootElement.ValueKind == JsonValueKind.Array);

            var list = new List<string>();

            var objCount = 0;
            var imageCount = 0;
            var maxX = 0;
            var maxY = 0;

            void readImage(JsonElement elem)
            {
                if (elem.TryGetProperty("image", out var image))
                {
                    ++imageCount;
                }

                if (elem.TryGetProperty("sheet_x", out var xProp) && xProp.TryGetInt32(out var x))
                {
                    maxX = Math.Max(maxX, x);
                }

                if (elem.TryGetProperty("sheet_y", out var yProp) && yProp.TryGetInt32(out var y))
                {
                    maxY = Math.Max(maxY, y);
                }

                if (elem.TryGetProperty("unified", out var unified) && unified.GetString() is { } us)
                {
                    list.Add(us);
                }
            }

            foreach (var elem in doc.RootElement.EnumerateArray())
            {
                Debug.Assert(elem.ValueKind == JsonValueKind.Object);

                readImage(elem);

                if (elem.TryGetProperty("skin_variations", out var skinVariations))
                {
                    foreach (var variation in skinVariations.EnumerateObject())
                    {
                        readImage(variation.Value);
                    }
                }
                ++objCount;
            }

            Console.WriteLine("obj count: " + objCount);
            Console.WriteLine("json image count:" + imageCount);
            Console.WriteLine("unified list count:" + list.Count);
            Console.WriteLine((maxX, maxY));
        }

        public static void CheckSkinVariations(JsonDocument doc)
        {
            Debug.Assert(doc.RootElement.ValueKind == JsonValueKind.Array);

            var list = new List<string>();

            var regSkinTone = new Regex(@"\-1F3F[B-F]");
            var regFe0f = new Regex(@"\-FE0F");

            int getSkinTone(Match m) => m.Value.Last() - 'B';

            string readUnified(JsonElement elem)
            {
                if (elem.TryGetProperty("unified", out var unified) && unified.GetString() is { } us)
                {
                    return us.ToUpper();
                }
                throw new Exception("来ないはず");
            }

            foreach (var elem in doc.RootElement.EnumerateArray())
            {
                Debug.Assert(elem.ValueKind == JsonValueKind.Object);

                var baseUnified = readUnified(elem);

                if (elem.TryGetProperty("skin_variations", out var skinVariations))
                {
                    var specialPattern = false;

                    var offset = 0;
                    foreach (var variation in skinVariations.EnumerateObject())
                    {
                        var unified = readUnified(variation.Value);

                        var m = regSkinTone.Matches(unified);
                        var mc = m.Count;

                        if (mc == 1)
                        {
                            // skin tone が1個のやつ、1F3FB～1F3FF が漏れなくこの順で並んでる。
                            var tone1 = getSkinTone(m[0]);

                            if (offset != tone1)
                            {
                                Console.WriteLine("来ないはず");
                            }
                        }
                        else if (mc == 2)
                        {
                            var tone1 = getSkinTone(m[0]);
                            var tone2 = getSkinTone(m[1]);

                            var offsetFromTone = tone2 + 5 * tone1;

                            if (baseUnified is "1F46B" or "1F46C" or "1F46D")
                            {
                                // やべーやつらのオフセットだけ計算難しい…
                                // tone1 == tone2 なのを除いて並んでる。
                                // 一応計算で出せはするみたいなので、
                                //
                                // 1F46B → 1F469-200D-1F91D-200D-1F468
                                // 1F46C → 1F468-200D-1F91D-200D-1F468
                                // 1F46D → 1F469-200D-1F91D-200D-1F469
                                //
                                // の3文字を追加で入れておけば絵は出せそう。
                                offsetFromTone = 5 + (tone1, tone2) switch
                                {
                                    (0, 1) => 0,
                                    (0, 2) => 1,
                                    (0, 3) => 2,
                                    (0, 4) => 3,
                                    (1, 0) => 4,
                                    (1, 2) => 5,
                                    (1, 3) => 6,
                                    (1, 4) => 7,
                                    (2, 0) => 8,
                                    (2, 1) => 9,
                                    (2, 3) => 10,
                                    (2, 4) => 11,
                                    (3, 0) => 12,
                                    (3, 1) => 13,
                                    (3, 2) => 14,
                                    (3, 4) => 15,
                                    (4, 0) => 16,
                                    (4, 1) => 17,
                                    (4, 2) => 18,
                                    (4, 3) => 19,
                                    _ => -1,
                                };
                            }

                            if (offset != offsetFromTone)
                            {
                                Console.WriteLine("来ないはず");
                            }
                        }

                        if (mc > 2)
                        {
                            Console.WriteLine("RGI 内に2個以上の skin tone の入ったシーケンスないはず");
                        }

                        var variationRemoved = regSkinTone.Replace(unified, "");

                        if (baseUnified != variationRemoved)
                        {
                            // もし差があるとしたら、
                            // まず、base 側の2文字目が FE0F のパターン。
                            var firstFe0fRemoved = regFe0f.Replace(baseUnified, m => m.Index <= 5 ? "" : m.Value);

                            if (firstFe0fRemoved != variationRemoved)
                            {
                                // それでも差があるやつ
                                // 👫 みたいにポリコレ仕様が入る前からある「固定の性別・固定の肌色」に1符号点割当たってるやつだと思う。
                                specialPattern = true;

                                // そのやべーやつは 200D-1F91D-200D (ZWJ 🤝 ZWJ) を含むはず。
                                if (!unified.Contains("200D-1F91D-200D"))
                                {
                                    Console.WriteLine("来ないはず");
                                }
                            }
                        }

                        ++offset;
                    }

                    if (specialPattern)
                    {
                        if (baseUnified is not "1F46B" and not "1F46C" and not "1F46D")
                        {
                            Console.WriteLine("やべーやつは 👫 (IF46B) 👬 (1F46C) 👭 (1F46D) の3文字だけっぽい");
                        }
                    }
                }
            }
        }
    }
}
