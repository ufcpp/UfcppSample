using BenchmarkDotNet.Running;

namespace VirtualCall
{
    /// <summary>
    /// 仮想メソッド呼び出しが絡んだ時のインライン展開状況の確認。
    ///
    /// <see cref="A"/>、<see cref="B"/>の中身を見てもらっての通り、やってる処理自体は全く同じなので、
    /// 性能差が出るとしたらインライン展開されるかどうかの問題。
    ///
    /// 比較対象:
    /// <see cref="BenchmarkCode.DirectA"/> … A 型変数に A 型インスタンス(<see cref="A.Value"/>にはvirtualが付いてない)
    /// <see cref="BenchmarkCode.DirectB"/> … B 型変数に B 型インスタンス(<see cref="B.Value"/>はvirtual)
    /// <see cref="BenchmarkCode.InterfaceA"/> … インターフェイス変数に A 型インスタンス
    /// <see cref="BenchmarkCode.InterfaceB"/> … インターフェイス変数に B 型インスタンス
    /// <see cref="BenchmarkCode.GenericA"/> … ジェネリック変数に A 型インスタンス
    /// <see cref="BenchmarkCode.GenericB"/> … ジェネリック変数に B 型インスタンス
    ///
    /// 実行すると以下のような感じに。
    ///      Method |        Mean |    StdErr |     StdDev |
    /// ----------- |------------ |---------- |----------- |
    ///     DirectA |  70.2508 ns | 0.7156 ns |  3.5778 ns |
    ///     DirectB | 205.1713 ns | 1.0471 ns |  4.0555 ns |
    ///  InterfaceA | 276.4652 ns | 2.7201 ns | 11.2154 ns |
    ///  InterfaceB | 272.4594 ns | 1.5071 ns |  5.8370 ns |
    ///    GenericA | 274.8484 ns | 1.0688 ns |  4.1394 ns |
    ///    GenericB | 275.0965 ns | 1.6639 ns |  6.4443 ns |
    ///
    /// 要するに、<see cref="A"/>型の変数/引数に直接<see cref="A"/>型のインスタンスを入れている場合だけインライン展開が掛かってる。
    /// インターフェイスを介したり、virtual がついていたりするとインライン展開できない。
    ///
    /// InlinedOrNot プロジェクトの方にも書いた通り、原則的に、仮想呼び出しになっているものはインライン展開できない。
    /// ただ、コンパイル時に具体的な型が確定しているようなものは、仮想でない呼び出しに変換する最適化が掛かる(結果的にインライン展開も掛かる)。
    ///
    /// この最適化が掛かるのも JIT の際。
    /// IL 命令的には <see cref="Target.CallA(A)"/> 内の <see cref="A.Value"/> 呼び出しも callvirt 命令(仮想呼び出し)になってる。
    /// というか、クラスのインスタンスに対するメソッド呼び出しは全部 callvirt になってる。
    /// けど、JIT 的に「明らかに仮想な必要ない」と判断したものは通常の呼び出しに変換される。
    ///
    /// ここで、ジェネリックを使った場合はどうなのかという話もある。
    /// 「<see cref="Target.Call{A}(A)"/>の呼び出しは<see cref="A"/>型の変数に<see cref="A"/>型のインスタンスが入っていること確定ではないのか」ということになるんだけども…
    /// この辺りは、ジェネリックがどう展開されているかが絡むのでまた次節で改めて。
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<BenchmarkCode>();
        }
    }
}
