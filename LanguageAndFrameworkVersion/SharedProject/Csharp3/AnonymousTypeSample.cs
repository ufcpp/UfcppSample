namespace VersionSample.Csharp3
{
    /// <summary>
    /// 匿名型も割とシンプルな構文糖衣の類。
    /// クラスが1個自動生成されるだけ。
    /// .NET 2.0 で問題なく動く。
    /// </summary>
    public class AnonymousTypeSample
    {
        public static void X()
        {
            // 匿名型
            var a = new
            {
                X = 3,
                Y = 5,
            };
        }

        public static void ApproxSameAsX()
        {
            // 匿名クラス
            var a = new CompilerGeneratedAnonymousClassA(3, 5);
        }

        private class CompilerGeneratedAnonymousClassA
        {
            private readonly int _x;
            private readonly int _y;

            public CompilerGeneratedAnonymousClassA(int v1, int v2)
            {
                _x = v1;
                _y = v2;
            }

            public int X { get { return _x; } }
            public int Y { get { return _y; } }

            public override bool Equals(object obj)
            {
                var x = obj as CompilerGeneratedAnonymousClassA;
                if (x == null) return false;
                return X == x.X && Y == x.Y;
            }

            public override int GetHashCode()
            {
                return X.GetHashCode() ^ Y.GetHashCode();
            }

            public override string ToString()
            {
                return string.Format("{{ X = {0}, Y = {1} }}", X, Y);
            }
        }
    }
}
