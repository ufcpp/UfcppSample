using System;
using System.Collections.Generic;

namespace VersionSample.Csharp3
{
    /// <summary>
    /// 型推論、初期化子、ラムダ式などは、割かし単純な構文糖衣でライブラリ非依存。
    /// 当然、.NET 2.0 上で動く。
    /// C# 2.0 相当のコードへの展開も割とシンプル。
    /// </summary>
    public class SyntaxSugarSample
    {
        public static void X()
        {
            // 変数の型推論
            var n = 1;
            var x = 1.0;

            // 配列の型推論
            var items = new[] { "one", "two", "three" };

            // ラムダ式
            var filtered = Where(items, s => s.Length > 3);

            foreach (var item in filtered)
            {
                Console.WriteLine(item);
            }

            // オブジェクト初期化子
            var p = new Point
            {
                X = 10,
                Y = 20,
            };

            // コレクション初期化子
            var list = new List<Point>
            {
                new Point(1, 2),
                new Point(3, 5),
                new Point(8, 13),
                new Point(21, 34),
            };
        }

        public static void SameAsX()
        {
            // 変数の型推論
            int n = 1;
            double x = 1.0;

            // 配列の型推論
            string[] items = new string[] { "one", "two", "three" };

            // ラムダ式
            var filtered = Where(items, delegate(string s) { return s.Length > 3; });

            foreach (var item in filtered)
            {
                Console.WriteLine(item);
            }

            // オブジェクト初期化子
            var p = new Point();
            p.X = 10;
            p.Y = 20;

            // コレクション初期化子
            var list = new List<Point>();
            list.Add(new Point(1, 2));
            list.Add(new Point(3, 5));
            list.Add(new Point(8, 13));
            list.Add(new Point(21, 34));
        }

        private static IEnumerable<string> Where(IEnumerable<string> source, System.Predicate<string> predicate)
        {
            foreach (var x in source)
                if (predicate(x))
                    yield return x;
        }
    }

    public class Point
    {
        // プロパティの自動実装
        public int X { get; set; }
        public int Y { get; set; }

        public Point() { }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class ApproxSameAsPoint
    {
        private int _x;
        public int X { get { return _x; } set { _x = value; } }

        private int _y;
        public int Y { get { return _y; } set { _y = value; } }

        public ApproxSameAsPoint() { }

        public ApproxSameAsPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
