using System;
using System.Linq;
using System.Threading.Tasks;

namespace DisposePattern.Tests
{
    /// <summary>
    /// ちゃんと正しく Dispose を管理できている例。
    /// </summary>
    /// <remarks>
    /// 最低限の寿命で Dispose してるので、<see cref="Counter.Max"/>はせいぜい並列度分までしか増えないはず。
    /// パフォーマンスを考えるとちゃんと Dispose しろという話ではあるんだけど…
    ///
    /// この例のはそんなに複雑じゃない(await WhenAll するだけでいい)から管理が楽だけど、実際にはもっと面倒なこともある。
    /// それに、呼び忘れがないか注意を払い続けるのは結構しんどい
    /// (C++でnew/deleteを自前管理してた頃を彷彿とさせる)。
    /// GCの楽さに慣れてしまった今となっては、この自前 Dispose 管理が旧時代的に見えてなおつらい。
    /// </remarks>
    class ManualDispose
    {
        public static void Test()
        {
            var counter = new Counter();
            var r = new Random();

            Task.WhenAll(Enumerable.Range(0, Constants.Parallelism).Select(_ => RunTask(counter, new Random()))).Wait();

            Console.WriteLine($"手動 Dispose, max: {counter.Max}, current: {counter.Current}");
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

            // 正しく子タスクが全部終わったことを確認してから s を Dispose しないといけない
            s.Dispose();
        }

        // s の Dispose 債務はこっち側にあるのか、呼び出し元にあるのかが不明
        static async Task RunSubTask(SampleDisopsable s, TimeSpan delayTime)
        {
            await Task.Delay(delayTime);
            // この例の場合は呼び出し元側でちゃんと管理しているからここで Dispose を呼んじゃダメ
        }
    }
}
