using System.Collections.Generic;

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
        }

        public static void SameAsX()
        {
            // 展開方法も、オブジェクト初期化子と似たようなもの。
            // 単に、代入がワンセットの式で書けるだけ。
            var dic = new Dictionary<string, int>();
            dic["one"] = 1;
            dic["two"] = 2;
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
