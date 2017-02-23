using System.Runtime.CompilerServices;

namespace InlinedOrNot
{
    /// <summary>
    /// 計測・比較したい対象。
    /// </summary>
    public class Target
    {
        // 普通に定義。このくらいの中身だと確実にインライン展開される
        public static int Inlining(int x, int y) => x + y;

        // インライン展開をわざわざ抑止
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int NoInlining(int x, int y) => x + y;
    }
}
