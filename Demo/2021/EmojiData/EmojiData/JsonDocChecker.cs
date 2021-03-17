using System;
using System.Collections.Generic;
using System.Diagnostics;
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

                    foreach (var variation in skinVariations.EnumerateObject())
                    {
                        var unified = readUnified(variation.Value);

                        var m = regSkinTone.Matches(unified);
                        var mc = m.Count;

                        if(mc > 2)
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
