using BenchmarkDotNet.Running;
using System.Linq;

namespace GuardedDevirt
{
    /// <summary>
    /// .NET Core 3.0 (向けだと思う。たぶん)で、以下のような最適化が入るみたい。
    /// https://github.com/dotnet/coreclr/blob/master/Documentation/design-docs/GuardedDevirtualization.md
    ///
    /// ある程度来る型がわかっているときに、仮想呼び出しを if 分岐に置き換えて通常のメソッド呼び出し(インライン展開可能)に変える手法。
    ///
    /// 以下の状況下では有効:
    /// - if の数がそんなに増えない
    /// - 確率に偏りがあって(大体は1つの型しか来なくて、たまに別の型来るだけ)、分岐予測が効きやすい
    /// - devirtualization が効いたらインライン展開される程度のサイズの関数
    ///
    /// これの有効性の確認用に、手作業で似たような if 展開をやってみてベンチマークを取ってみる。
    /// <see cref="CallBenchmark"/>
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            var b = new X();
            b.Setup();
            b.BranchesB();
#else
            BenchmarkRunner.Run<CallBenchmark>();
#endif
        }
    }
}
