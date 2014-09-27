using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Build2014.Csharp6
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

        // 宣言式のおかげで out 引数を使う場合でも式1個で書けたりする
        // この例はさすがにちょっとやりすぎかも。普通に yield return 使う方が読みやすい
        public static IEnumerable<Point> FromJson(JArray array) =>
            from json in array
            select (TryGetInt(json["x"], out var x) // 式の中で変数宣言できるように(宣言式)
                && TryGetInt(json["y"], out var y))
                ? new Point(x, y)
                : null
                into p
            where p != null
            select p;

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
