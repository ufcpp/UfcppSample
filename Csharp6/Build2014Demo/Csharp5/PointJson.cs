using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Build2014.Csharp5
{
    static class PointJson
    {
        // 「return 式1個だけ;」なメソッド
        // ラムダ式の場合は「() => 式;」みたいに { return } を省略できるのに…
        public static JArray ToJson(IEnumerable<Point> points)
        {
            return new JArray(
                from p in points
                where p != null
                select new JObject
                {
                    { "x", p.X },
                    { "y", p.Y },
                });
        }

        public static IEnumerable<Point> FromJson(JArray array)
        {
            foreach (var json in array)
            {
                if (json == null) continue;

                // out 引数で使うために変数宣言
                // こいつのために、「式」にできなくなる
                int x, y;
                if (TryGetInt(json["x"], out x)
                    && TryGetInt(json["y"], out y))
                {
                    yield return new Point(x, y);
                }
            }
        }

        private static bool TryGetInt(JToken json, out int value)
        {
            var x = json as JValue;
            var isInt = x != null && x.Type == JTokenType.Integer;
            value = isInt ? x.ToObject<int>() : 0;
            return isInt;
        }

        public static string ArrayToString(this IEnumerable<Point> points)
        {
            return "[" + string.Join(", ", points) + "]";
        }

        public static IEnumerable<Point> ParseArray(string json)
        {
            return FromJson(JArray.Parse(json));
        }
    }
}
