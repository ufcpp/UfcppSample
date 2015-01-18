using System;
using System.Collections.Generic;

namespace Csharp6.Csharp5
{
    class Program
    {
        public static void Run()
        {
            var points = new List<Point>
            {
                // C# 3.0 のコレクション初期化子
                // List.Add(Point) が呼ばれてる
                // 1行1行 new Point を書くのがめんどいことがある
                new Point(3, 4),
                new Point(-1, 0),
                new Point(5, -2),
                new Point(7, 6),
                new Point(0, 0)
            };

            // Console. やら PointJson. やら、横に間延びしがち
            Console.WriteLine(points.ArrayToString());
            var json = PointJson.ToJson(points).ToString();
            Console.WriteLine(json);
            Console.WriteLine(PointJson.ParseArray(json).ArrayToString());
        }
    }
}
