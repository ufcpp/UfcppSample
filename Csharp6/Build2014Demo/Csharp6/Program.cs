using System.Collections.Generic;
using System.Console;
using Build2014.Csharp6.PointJson;

namespace Build2014.Csharp6
{
    class Program
    {
        public static void Run()
        {
            var points = new List<Point>
            {
                // List.Add を呼んでいることには変わりないのだけども
                // C#ｖNext では拡張メソッドでも OK になった
                // ここで実際に呼ばれているのは PointExtensions.Add(this IList<Point>, int, int)
                { 3, 4 },
                { -1, 0 },
                { 5, -2 },
                { 7, 6 },
                { 0, 0 }
            };

            // using System.Console ってやると、その C# ファイル内では Console. を省略できる
            // 拡張メソッド同様、実際にどこで定義されているメソッドかわかりにくくなるって問題もあるのでやたらと使いたい機能ではないけども、便利な場面も結構あるはず
            WriteLine(points.ArrayToString());
            var json = ToJson(points).ToString();
            WriteLine(json);
            WriteLine(ParseArray(json).ArrayToString());
        }
    }

    static class PointExtensions
    {
        public static void Add(this IList<Point> list, int x, int y) => list.Add(new Point(x, y));
    }
}
