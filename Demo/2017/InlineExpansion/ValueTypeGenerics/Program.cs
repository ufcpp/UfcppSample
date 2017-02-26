namespace ValueTypeGenerics
{
    /// <summary>
    /// 01～03のまとめ:
    /// 1. 中身が小さいメソッドは、インライン展開すると数倍速くなる
    /// 2．仮想呼び出しがだとインライン展開できなくなる/JITの最適化で仮想呼び出しが消えることがある
    /// 3. ジェネリックは、参照型の場合にはコード共有、値型の場合には個別展開になる
    ///
    /// これらを併せて、
    /// 値型ジェネリックを使って汎用処理を書く → 個別展開される → 仮想呼び出しが消える → インライン展開可能
    /// という最適化が期待できる。
    ///
    /// ということで、以下のような例を考える:
    /// <see cref="System.Linq.Enumerable.Sum(System.Collections.Generic.IEnumerable{int})"/> は、見ての通り、ジェネリック実装になっていない。
    /// (int, long, ...など、型ごとに実装がある。)
    /// でも、総和計算なんて、初期値(0)と2項演算子さえあればどんな型に対しても計算できるはず。
    /// 実際、<see cref="IGroup"/>みたいなインターフェイスを使えば汎用に総計処理できるはず
    /// (例として、総和<see cref="AddGroup"/>と総乗<see cref="MulGroup"/>を用意)だし、
    /// <see cref="System.Linq.Enumerable.Aggregate{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TSource, TSource})"/>でそんな感じの処理も書ける。
    /// ただ、インターフェイスやデリゲートを介すると、上記の「インライン展開」問題で、数倍遅くなってしまう。
    /// これに対して、01～03から、値型ジェネリックを使った最適化が期待できるので、それを試してみる。
    /// 
    /// 試しに書いてみたもの:
    /// <see cref="BenchmarkCode.EmbeddedAdd"/> … + 演算子をベタ書きして(総和と総乗を個別に実装して)計算するもの
    /// <see cref="BenchmarkCode.InterfaceAdd"/> … (参考)引数にインターフェイス渡して総和を計算。インライン展開ができない
    /// <see cref="BenchmarkCode.TypeClassAdd"/> … 値型ジェネリックを使った最適化を期待したもの
    /// 同様に、語尾が Mul のもので総乗を計算。
    ///
    /// 実行すると以下のような感じに。
    ///        Method |        Mean |    StdDev | Allocated |
    /// ------------- |------------ |---------- |---------- |
    ///   EmbeddedAdd |  22.5229 ns | 0.2941 ns |       0 B |
    ///  InterfaceAdd | 140.9058 ns | 1.0286 ns |      12 B |
    ///  TypeClassAdd |  31.8188 ns | 0.2503 ns |       0 B |
    ///   EmbeddedMul |  32.4480 ns | 0.1794 ns |       0 B |
    ///  InterfaceMul | 140.0370 ns | 1.1462 ns |      12 B |
    ///  TypeClassMul |  33.0551 ns | 0.3279 ns |       0 B |
    ///
    /// 期待通りに、
    /// - インターフェイスを介するとインライン展開されていない(数倍遅い)
    /// - 値型ジェネリックを使うと、汎用処理であるにもかかわらず、ちゃんとインライン展開が掛かっている(ベタ書き実装と遜色ない)
    /// となってる。
    /// </summary>
    class Program
    {
        static void Main()
        {
            BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchmarkCode>();
        }
    }
}
