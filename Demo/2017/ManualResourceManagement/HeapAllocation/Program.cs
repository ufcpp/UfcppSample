using BenchmarkDotNet.Running;

namespace HeapAllocation
{
/* 手元の環境でのベンチマーク実行結果の一例

            Method |        Mean |     StdDev |   Gen 0 | Allocated |
------------------ |------------ |----------- |-------- |---------- |
            Struct |   5.5147 us |  0.0326 us |       - |      24 B |
 GarbageCollection |  35.2030 us |  0.1047 us | 55.8919 | 240.05 kB |
           Malloc0 | 763.0477 us |  1.9576 us |       - |      32 B |
            Malloc | 772.7615 us |  6.8421 us |       - |      32 B |
   LockPoolPointer | 539.6967 us | 16.9640 us |       - |      32 B |
    CasPoolPointer | 212.1614 us |  1.3458 us |       - |      32 B |
  LocalPoolPointer |  85.5372 us |  0.4944 us |       - |      26 B |
 */

    /// <summary>
    /// .NET の GC (Mark and Sweep 方式)がどのくらい高性能かというのをベンチマーク取って調べる。
    /// Javaとか.NETのヒープ確保はほんとかなり速い。
    /// ちょっとやそっと自前メモリ管理を頑張ってもまず勝てない。
    ///
    /// まず、C++のnew/deleteみたいな処理と比べたら、GCは(たとえGCが発生しても)20倍くらい速い。
    /// (元々、そんなに頻繁にnew/deleteしちゃダメ)
    ///
    /// なので、メモリ プール(最初に大き目の領域を new して、その中でメモリを自前管理)を作ったとして、それでも、単純な実装だとGCの方が1桁速い。
    /// これでも、かなり用途を絞った(参照カウントとかが必要ない)実装なので速い方。
    /// 参照カウントとかが必要になるとさらに遅くなるはず。
    ///
    /// 自前実装だから苦しいという面も多少あるだろうものの、原理的にもMark and Sweepはだいぶ速いはず。
    ///
    /// 要するに、管理対象がメモリだけなんだったら、わざわざ自前で頑張る必要性は皆無という話。
    /// 問題はメモリ以外のリソース(ファイルとかネットワークとか)なんだけど、それはまた別途説明。
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            Test.Run();
            BenchmarkRunner.Run<AllocationBenchmark>();
        }
    }
}
