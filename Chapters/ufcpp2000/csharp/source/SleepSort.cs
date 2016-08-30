using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace SleepSort
{
    /// <summary>
    /// 例のネタ（http://d.hatena.ne.jp/gfx/20110519/1305810786）を C# で実装してみたよ。というやつ。
    /// 
    /// .NET でスレッドを直接立てたら負けかなと思っている。
    /// 非同期シーケンスを、全要素の列挙完了するまで Wait しても負けかなと思っている。
    /// 
    /// ・スレッドを直接立てない
    /// 要素の数だけスレッドを立てるとか、何それ怖い。
    /// Timer（マニュアル スレッドではなく、スレッド プール）を使います。
    /// 
    /// ・全要素の列挙完了まで Wait しない
    /// IObservable を使って push 型に受信。
    /// 実行してみればわかる通り、全部ソートし終えてから一気に表示されるのではなく、
    /// 1要素ずつ Console.WriteLine されます。
    /// 
    /// Reactive Extensions を利用。
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var data = GenerateTestData();
            var result = Sort(data, x => x);

            var e = new AutoResetEvent(false);

            result.Subscribe(x => Console.WriteLine(x), () => e.Set());

            e.WaitOne();
        }

        /// <summary>
        /// うわさのスリープ ソート。
        /// </summary>
        /// <typeparam name="T">ソート対象の要素の型。</typeparam>
        /// <param name="data">ソート対象。</param>
        /// <param name="toOrder">T 型から順序を表す整数に変換。</param>
        /// <returns>結果受け取り用の Observable</returns>
        static IObservable<T> Sort<T>(IEnumerable<T> data, Func<T, int> toOrder)
        {
            const int Weight = 50;

            var results = new Subject<T>();
            var maxTime = int.MinValue;

            foreach (var x in data)
            {
                var local = x;
                var time = toOrder(x) * Weight;
                maxTime = Math.Max(maxTime, time);

                TaskEx.Delay(time).ContinueWith(t => { results.OnNext(local); });
            }

            TaskEx.Delay(maxTime + Weight).ContinueWith(t => results.OnCompleted());

            return results;
        }

        /// <summary>
        /// テスト データの生成。
        /// </summary>
        /// <returns>テスト データ。</returns>
        private static IEnumerable<int> GenerateTestData()
        {
            const int N = 10000;
            var rand = new Random();
            var data = Enumerable.Range(0, N)
                //.OrderBy(x => rand.Next());
                .Select(x => rand.Next(0, 100));
            return data;
        }
    }

    /// <summary>
    /// Async CTP の車輪の再発名。
    /// これだけのためにアセンブリ参照増やしたくないんで自作。
    /// </summary>
    class TaskEx
    {
        /// <summary>
        /// 引数で指定した時間だけ待つ。
        /// </summary>
        /// <param name="milliseconds">待つ時間[ミリ秒]。</param>
        /// <returns>Wait するために Task を返す。</returns>
        public static Task Delay(int milliseconds)
        {
            var tcs = new TaskCompletionSource<bool>();

            Timer t = null;
            t = new Timer(self =>
            {
                t.Dispose();
                tcs.TrySetResult(true);
            }, t, milliseconds, Timeout.Infinite);

            return tcs.Task;
        }

    }
}
