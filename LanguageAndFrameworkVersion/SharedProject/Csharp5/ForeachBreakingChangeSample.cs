using System;

namespace VersionSample.Csharp5
{
    /// <summary>
    /// C# には珍しい破壊的変更の1つ。
    /// foreach (var x in ...) の、x のスコープが変わった。
    ///
    /// これは、生成する IL が変更されたもので、単純に C# コンパイラーだけの問題。
    /// ビルド時にどのコンパイラーを使ったかによって挙動が変わって、実行時にどの .NET ランタイムを使っているかにはよらない。
    /// 逆に言うと、.NET 4 移行を使っていたとしても、古いコンパイラーでビルドすると古い挙動になるので注意。
    /// </summary>
    public class ForeachBreakingChangeSample
    {
        delegate void Action();

        public static void X()
        {
            var data = new[] { 1, 2, 3, 4, 5 };

            Action a = null;

            foreach (var x in data)
            {
                // x が、
                // C# 4.0 まで: foreach の { } 外で作られる(1個だけになる)
                // C# 5.0 から: foreach の { } 内で作られる(ループのたびに別になる)

                // この違いが問題になる場面はそんなに多くないものの…
                // ラムダ式での変数キャプチャとかやると挙動に差が出る。
                a += () => Console.WriteLine(x);
            }

            a();
/*
C# 4.0 までの結果
5
5
5
5
5

C# 5.0 移行での結果
1
2
3
4
5
*/
        }
    }
}
