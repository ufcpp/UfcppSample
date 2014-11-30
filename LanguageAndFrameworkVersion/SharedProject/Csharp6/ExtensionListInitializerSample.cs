#if Ver3 || (Ver2 && Plus)

using System.Collections.Generic;

namespace VersionSample.Csharp6
{
    /// <summary>
    /// 拡張メソッドでのコレクション初期化子は、
    /// 拡張メソッドさえ使えるバージョンであれば必ず使える。
    /// </summary>
    public static class ExtensionListInitializerSample
    {
        public static void X()
        {
            // コレクション初期化子の Add が拡張メソッドでもよくなった。
            var list = new List<Point>
            {
                { 1, 2 },
                { 3, 4 },
            };
        }

        public static void SameAsX()
        {
            // 拡張メソッドの Add が呼ばれる。
            // ここではさらに、拡張メソッド自体も展開(通常の静的メソッド呼び出しに)してる。
            var list = new List<Point>();
            PointExtensions.Add(list, 1, 2);
            PointExtensions.Add(list, 3, 4);
        }
    }

    public static class PointExtensions
    {
        public static void Add(this List<Point> list, int x, int y)
        {
            list.Add(new Point(x, y));
        }
    }
}

#endif
