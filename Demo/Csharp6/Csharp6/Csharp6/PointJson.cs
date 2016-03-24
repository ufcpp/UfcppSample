using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Csharp6.Csharp6
{
    static class PointJson
    {
        // 式1個だけな関数(メソッド、プロパティのgetter、演算子)は、ラムダ式風の => を使った書き方ができるようになった
        public static JArray ToJson(IEnumerable<Point> points) =>
            new JArray(
                from p in points
                where p != null
                select new JObject
                {
                    { "x", p.X },
                    { "y", p.Y },
                });

        public static IEnumerable<Point> FromJson(JArray array)
        {
            foreach (var json in array)
            {
                if (json == null) continue;

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

        // ラムダ式風の関数定義は、こういう短い式1個だけなメソッドで使うのがよさそう
        public static string ArrayToString(this IEnumerable<Point> points) => "[" + string.Join(", ", points) + "]";

        public static IEnumerable<Point> ParseArray(string json) => FromJson(JArray.Parse(json));
    }
}
