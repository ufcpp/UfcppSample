using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;

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
    }
}
