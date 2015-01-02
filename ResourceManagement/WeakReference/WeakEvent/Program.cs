namespace WeakReferenceSample.WeakEvent
{
    using System;
    using System.Reactive.Subjects;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static void Main()
        {
            RunAsync(true).Wait();
            RunAsync(false).Wait();
        }

        private const int Interval = 100;

        private static async Task RunAsync(bool manualDispose)
        {
            if (manualDispose) Console.WriteLine("ちゃんと Dispose");
            else Console.WriteLine("GC 任せ");

                // イベントを、
                // d1: 通常のイベント購読
                // d2: 弱イベント購読
                var x = new Subject<int>();
            var d1 = x.Subscribe(i => Console.WriteLine("subscribe " + i));
            var d2 = x.WeakSubscribe(i => Console.WriteLine("weak subscribe " + i));
            var cts = new CancellationTokenSource();
            var t = EventSourceLoop(x, cts.Token);

            // イベントが飛んでくる間隔の3倍待つ → 3回イベントが来る
            await Task.Delay(3 * Interval);

            if (manualDispose)
            {
                // ちゃんと Dispose。
                // 当たり前だけども、以後、イベントは受け取らなくなる。
                d1.Dispose();
                d2.Dispose();
            }
            else
            {
                // Dispose 忘れたままオブジェクトを捨てる。
                // d1 は、Subscribe 内で参照を握っているので GC 対象にならない。メモリ リーク。
                // d2 は、WeakSubscribe 内は弱参照なので、こっちの参照なくせば GC 対象。
                // 以後、イベントは subscribe 側にだけ届く。
                d1 = null;
                d2 = null;
                GC.Collect();
            }

            // 同じく3回分待つ
            await Task.Delay(300);

            cts.Cancel();
            await t;
        }

        // イベントを飛ばし続けるループ
        static async Task EventSourceLoop(IObserver<int> observer, CancellationToken ct)
        {
            for (var i = 0; !ct.IsCancellationRequested; ++i)
            {
                observer.OnNext(i);
                await Task.Delay(Interval);
            }
        }
    }
}
