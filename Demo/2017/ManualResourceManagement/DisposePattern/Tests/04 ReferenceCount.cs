using System;
using System.Linq;
using System.Threading.Tasks;

namespace DisposePattern.Tests
{
    /// <summary>
    /// <see cref="StructCantHaveFinalizer"/>とかとやってることは一緒。
    /// 参照カウント処理を足した。
    /// </summary>
    /// <remarks>
    /// HeapAllocationの例で示した通り、参照カウント自体そこそこ処理負担はある。
    /// それでも、Finalizerに任せるよりはマシな負担だし、構造体を使えるのも大きい。
    ///
    /// 現状のC#だとコンパイラーのサポートがないので、Analyzerを書かないと静的チェックが効かないものの、そのAnalyzerを作る価値はあるかも。
    /// もしかしたらコンパイラーのサポートを入れるべきかもしれない。
    /// </remarks>
    class ReferenceCount
    {
        public static void Test()
        {
            var counter = new Counter();
            var r = new Random();

            Task.WhenAll(Enumerable.Range(0, Constants.Parallelism).Select(_ => RunTask(counter, new Random()))).Wait();

            Console.WriteLine($"参照カウント, max: {counter.Max}, current: {counter.Current}");
        }

        private static async Task RunTask(Counter counter, Random r)
        {
            var s = new SampleReferenceCount(counter).Init();

            await Task.WhenAll(
                RunSubTask(s.Share(), r.Time()),
                RunSubTask(s.Share(), r.Time()),
                RunSubTask(s.Share(), r.Time()),
                RunSubTask(s.Share(), r.Time()),
                RunSubTask(s.Share(), r.Time()),
                RunSubTask(s.Move(), r.Time())); // 1個Moveしたので、もう親側にRelease義務なし
        }

        // s の Dispose 債務はこっち側にあるのか、呼び出し元にあるのかが不明
        static async Task RunSubTask(SampleReferenceCount s, TimeSpan delayTime)
        {
            await Task.Delay(delayTime);

            // 値渡しで s を受け取ってるので、Release義務が発生
            // Releaseしないとコンパイルエラーになるべき(要Analyzer)
            s.Release();
        }
    }
}
