using System;
using System.Collections.Generic;

using static System.Math;

namespace VersionSample.Csharp6
{
    /// <summary>
    /// C# 6.0 の機能はかなりの部分、かなりシンプルな構文糖衣。
    /// .NET 2.0 上で余裕で動く。
    /// </summary>
    public class SyntaxSugarSample
    {
        public static void X()
        {
            var dic = new Dictionary<string, int>
            {
                ["one"] = 1,
                ["two"] = 2,
            };

            Console.WriteLine(nameof(dic) + " in " + nameof(X));
        }

        public static void SameAsX()
        {
            // 展開方法も、オブジェクト初期化子と似たようなもの。
            // 単に、代入がワンセットの式で書けるだけ。
            var dic = new Dictionary<string, int>();
            dic["one"] = 1;
            dic["two"] = 2;

            // nameof は普通に文字列リテラルに展開。
            // 要は、静的コード解析とかリファクタリングのターゲットにできるというだけの文字列リテラル。
            Console.WriteLine("dic" + " in " + "X");
        }

        public static double Length(double x, double y)
        {
            return Sqrt(x * x + y * y);
        }

        public static double SameAsLength(double x, double y)
        {
            // 単に Math. が省略できるだけ
            return Math.Sqrt(x * x + y * y);
        }

        public static void Y()
        {
            try
            {
            }
            catch (ArgumentException e) if (e.ParamName == "x")
            {
                // パラメーター名が x の時だけはエラー無視
            }

            // この構文は、これまでの C# では書く方法ない。
            // IL レベルでは、元々 catch 句にフィルターが書けた。その機能に C# も対応しただけ。
        }
    }

    public class Point
    {
        // getter-only 自動実装プロパティ
        public int X { get; } = 1;
        public int Y { get; } = 2;

        public Point() { }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        // expression-bodied プロパティ
        public int Size => X * Y;

        // expression-bodied メソッド
        // string-interpolation
        public override string ToString() => "(" + X + ", " + Y + ")";
    }

    public class ApproxSameAsPoint
    {
        private readonly int _x = 1;
        public int X { get { return _x; } }

        private readonly int _y = 2;
        public int Y { get { return _y; } }

        public ApproxSameAsPoint() { }

        public ApproxSameAsPoint(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int Size { get { return X * Y; } }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }
    }
}
