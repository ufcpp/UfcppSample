using BenchmarkDotNet.Running;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HeapAllocation
{
/* 手元の環境でのベンチマーク実行結果の一例
(AllocHGlobal, AllocHGlobalRefactored の2個は実行回数が他の100分の1。)

                 Method |        Mean |    StdDev |   Gen 0 | Allocated |
----------------------- |------------ |---------- |-------- |---------- |
                 Struct |   5.4580 us | 0.0388 us |       - |      24 B |
      GarbageCollection |  34.0301 us | 0.1920 us | 55.9245 | 240.05 kB |
           AllocHGlobal | 241.9010 us | 1.2387 us |       - |      32 B |
 AllocHGlobalRefactored | 247.5479 us | 3.3355 us |       - |      32 B |
         LockMemoryPool | 514.3064 us | 3.1241 us |       - |      32 B |
          CasMemoryPool | 344.0864 us | 6.5854 us |       - |      32 B |
*/

    /// <summary>
    /// .NET の GC (Mark and Sweep 方式)がどのくらい高性能かというのをベンチマーク取って調べる。
    /// Javaとか.NETのヒープ確保はほんとかなり速い。
    /// ちょっとやそっと自前メモリ管理を頑張ってもまず勝てない。
    ///
    /// まず、C++のnew/deleteみたいな処理と比べたら、GCは(たとえGCが発生しても)3桁くらい速い。
    /// (元々、そんなに頻繁にnew/deleteしちゃダメ)
    ///
    /// なので、メモリプールみたいなのを作って自前でメモリ領域管理したとして、それでも、単純な実装だとGCの方が1桁速かった。
    /// これでも、所有者が1人きりのメモリ(参照カウントとかが必要ない)なので速い方。
    /// 参照カウントとかが必要になるとさらに遅くなるはず。
    /// .NET ランタイムとかが内部的に頑張ってくれれば速くなるかというと、原理的にもMark and Sweepの方がだいぶ速いはず。
    ///
    /// 要するに、管理対象がメモリだけなんだったら、わざわざ自前で頑張る必要性は皆無という話。
    /// 問題はメモリ以外のリソース(ファイルとかネットワークとか)なんだけど、それはまた別途説明。
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            Test();
            BenchmarkRunner.Run<Allocation>();
        }

        /// <summary>
        /// 全部結果が同じかだけ確認。
        /// </summary>
        private static void Test()
        {
            const int N = 50;
            for (int i = 0; i < N; i++)
            {
                var a = Allocation.Struct(i);
                if (!a.Equals(Allocation.GarbageCollection(i))) throw new InvalidOperationException();
                if (!a.Equals(Allocation.AllocHGlobal(i))) throw new InvalidOperationException();
                if (!a.Equals(Allocation.AllocHGlobalRefactored(i))) throw new InvalidOperationException();
                if (!a.Equals(Allocation.LockMemoryPool(i))) throw new InvalidOperationException();
            }

            // プール実装だけは並列実行があっても大丈夫か要確認
            Task.WhenAll(Enumerable.Range(0, 10).Select(_ => Task.Run(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    var a = Allocation.Struct(i);
                    var b = Allocation.LockMemoryPool(i);
                    if (!a.Equals(b)) throw new InvalidOperationException();
                }
            }))).Wait();

            Task.WhenAll(Enumerable.Range(0, 10).Select(_ => Task.Run(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    var a = Allocation.Struct(i);
                    var b = Allocation.CasMemoryPool(i);
                    if (!a.Equals(b)) throw new InvalidOperationException();
                }
            }))).Wait();
        }
    }
}
