using BenchmarkDotNet.Running;

namespace HeapAllocation
{
    /* 手元の環境でのベンチマーク実行結果の一例
    (AllocHGlobal0, AllocHGlobal の2個はダントツで遅いことが分かり切っているのでループ回数が他の100分の1。)

            Method |        Mean |     StdDev |   Gen 0 | Allocated |
------------------ |------------ |----------- |-------- |---------- |
            Struct |   5.6062 us |  0.0544 us |       - |      24 B |
 GarbageCollection |  35.9424 us |  0.2510 us | 55.7292 | 240.05 kB |
     AllocHGlobal0 | 254.0772 us |  3.3096 us |       - |      32 B | ※100分の1
      AllocHGlobal | 256.7405 us |  3.6462 us |       - |      32 B | ※100分の1
   LockPoolPointer | 549.5252 us | 16.6442 us |       - |      32 B |
    CasPoolPointer | 213.0785 us |  0.7297 us |       - |      32 B |
  LocalPoolPointer |  93.1231 us |  0.3804 us |       - |      26 B |
    */

    /// <summary>
    /// .NET の GC (Mark and Sweep 方式)がどのくらい高性能かというのをベンチマーク取って調べる。
    /// Javaとか.NETのヒープ確保はほんとかなり速い。
    /// ちょっとやそっと自前メモリ管理を頑張ってもまず勝てない。
    ///
    /// まず、C++のnew/deleteみたいな処理と比べたら、GCは(たとえGCが発生しても)3桁くらい速い。
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
