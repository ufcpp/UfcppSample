using System;
using System.Linq;
using System.Threading.Tasks;

namespace DisposePattern.Tests
{
    /// <summary>
    /// Dispose 漏れがある場合の例。
    /// </summary>
    /// <remarks>
    /// <see cref="SampleDisopsable"/>はDisposeを呼ばなくてもデストラクター(Finalizer)でカウントダウンしているので、最終的に、GCさえ走れば<see cref="Counter.Current"/>は0に戻るはず。
    /// ただ、当然、無駄に寿命が伸びるんで、<see cref="Counter.Max"/>は増える。
    ///
    /// <see cref="GC.Collect"/>でGCを強制的に掛けているのでDisposeされずに残る時間はそこまで長くないけど、
    /// GC強制起動とか普通はやらない操作なので、本来はもっと長時間Disposeされずに残る。
    ///
    /// それに、Finalizerの呼び出し自体が実は結構負担高い。
    /// <see cref="SampleDisopsable.Dispose"/>内で<see cref="GC.SuppressFinalize(object)"/>を呼んでるのも、
    /// そもそもFinalizerが呼ばれないようにしてパフォーマンスを向上させるため。
    ///
    /// 後述(<see cref="StructCantHaveFinalizer"/>で説明するように、クラス(参照型)でしかこの手段(Finalizerに期待する)が使えないのも問題。
    /// </remarks>
    class Finalizer
    {
        public static void Test()
        {
            var counter = new Counter();
            var r = new Random();

            Task.WhenAll(Enumerable.Range(0, Constants.Parallelism).Select(_ => RunTask(counter, new Random()))).Wait();

            // GCを掛けて、Finalizer 完了待ちしないと Current が 0 にならない。
            GC.Collect(2, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"デストラクター任せ, max: {counter.Max}, current: {counter.Current}");
        }

        private static async Task RunTask(Counter counter, Random r)
        {
            var s = new SampleDisopsable(counter);

            await Task.WhenAll(
                RunSubTask(s, r.Time()),
                RunSubTask(s, r.Time()),
                RunSubTask(s, r.Time()),
                RunSubTask(s, r.Time()),
                RunSubTask(s, r.Time()),
                RunSubTask(s, r.Time()));

            // 「子タスクの誰かが Dispose 呼ぶんじゃないかなぁ」みたいな「にらめっこ」の結果、Dispose が呼ばれてないとする
        }

        // s の Dispose 債務はこっち側にあるのか、呼び出し元にあるのかが不明
        static async Task RunSubTask(SampleDisopsable s, TimeSpan delayTime)
        {
            await Task.Delay(delayTime);
            // この例の場合は呼び出し元側でちゃんと管理していると思っているので、Dispose は呼ばない
        }
    }
}
