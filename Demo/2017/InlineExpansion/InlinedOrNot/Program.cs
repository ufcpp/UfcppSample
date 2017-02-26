using BenchmarkDotNet.Running;

namespace InlinedOrNot
{
    /// <summary>
    /// インライン展開の有無でどのくらい性能差が出るかのベンチマーク。
    /// 足し算1個だけの処理をループするだけ。
    ///
    /// 比較対象:
    /// - <see cref="BenchmarkCode.ManuallyInlined"/>: (参考) ループ中に + をべた書き
    /// - <see cref="BenchmarkCode.Inlining"/>       : インライン展開あり(+ 1個だけなら普通はインライン展開される)
    /// - <see cref="BenchmarkCode.NoInlining"/>     : インライン展開なし(わざわざ抑止オプションを付けてる)
    ///
    /// 実行すると以下のような感じに。
    ///           Method |        Mean |    StdDev |
    /// ---------------- |------------ |---------- |
    ///  ManuallyInlined |  81.8159 ns | 3.0530 ns |
    ///         Inlining |  82.7913 ns | 3.4481 ns |
    ///       NoInlining | 243.7487 ns | 2.2184 ns |
    ///
    /// インライン展開があると、べた書きと同程度の性能になる。
    ///
    /// インライン展開しないものは3倍くらい遅い。
    /// ジャンプ命令が2回(呼び出し → 復帰)挟まったりするコストがかかってる。
    /// 他にも、レジスターの利用に制限が掛かったり、参照局所性的に不利だったりする。
    /// 中身が小さい時には無視できないコストになる。
    ///
    /// .NET の場合、インライン展開が掛かるには以下のような条件が必要
    /// - メソッドの中身が十分に小さい(32バイト程度)
    /// - whileとかswitchとかの制御構文が入っていない
    /// - 仮想呼び出しになっていない
    ///
    /// ちなみに、インライン展開が掛かるタイミングは JIT の際。
    /// C# コンパイラーが生成する IL は、インライン展開前の状態になってる。
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<BenchmarkCode>();
        }
    }
}
