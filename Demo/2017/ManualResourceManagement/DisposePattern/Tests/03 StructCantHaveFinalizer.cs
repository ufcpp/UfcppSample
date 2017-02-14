using System;
using System.Linq;
using System.Threading.Tasks;

namespace DisposePattern.Tests
{
    /// <summary>
    /// <see cref="Finalizer"/>の例と同じことを構造体でやるとどうなるか。
    /// すなわち、構造体でDispose漏れがある場合の例。
    /// </summary>
    /// <remarks>
    /// クラス(参照型)でしかFinalizerが使えないので、構造体でDispose漏れを起こしたらその時点でリソースリーク確定。
    /// GCを掛けようが何しようが、<see cref="Counter.Current"/>は増えっぱなし(リーク)。
    /// 
    /// この例(<see cref="Counter"/>を1個だけ持つ小さい型)だけでなく、構造体を使いたい場面は結構多いはず。
    /// 例えば、以下のように、小さいデータしか持たないものは構造体にしたい
    /// (Native相互運用する上で、ポインターとかハンドル値とかを1個だけ持つ型ってのは高頻度であり得る)。
    /// <code><![CDATA[
    /// struct Sample : IDisposable
    /// {
    ///     IntPtr pointerToNativeResource;
    ///     public void Disose() => ReleaseNativeResource();
    /// }
    /// ]]></code>
    /// そうしたいけどもできない理由が、ここで例示したような、Dispose漏れをFinalizerでカバーできないという問題。
    /// </remarks>
    class StructCantHaveFinalizer
    {
        public static void Test()
        {
            var counter = new Counter();
            var r = new Random();

            Task.WhenAll(Enumerable.Range(0, Constants.Parallelism).Select(_ => RunTask(counter, new Random()))).Wait();

            // GCを掛けたところで無駄。構造体はGC管理されてない。カウントダウン処理はどこからも実行されない。
            GC.Collect(2, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"構造体でDisose漏れ, max: {counter.Max}, current: {counter.Current}");
        }

        private static async Task RunTask(Counter counter, Random r)
        {
            var s = new SampleDisopsableStruct(counter);

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
        static async Task RunSubTask(SampleDisopsableStruct s, TimeSpan delayTime)
        {
            await Task.Delay(delayTime);
            // この例の場合は呼び出し元側でちゃんと管理していると思っているので、Dispose は呼ばない
        }
    }
}
