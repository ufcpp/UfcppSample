using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConsoleApp1._05_GeneralizedAsync
{
    class Performance
    {
        public static void Run()
        {
            // キャッシュを効かせるために1回空呼び
            _01.Run().Wait();
            _02.Run().Wait();

            // 実行時間を計測 ＆ ヒープ状況を観測
            void measure(Func<Task> f)
            {
                var sw = new Stopwatch();
                sw.Start();
                var mem = GC.GetTotalMemory(false);

                f().Wait();

                mem = GC.GetTotalMemory(false) - mem; // ヒープ確保があれば値が変わる(GC 発生で減ることもある)
                sw.Stop();
                Console.WriteLine($"elapsed time: {sw.Elapsed}, heap size: {mem}");
                sw.Reset();
            }

            // Task GetValue の方
            measure(_01.Run);

            // ValueTask GetValue の方
            // ヒープ確保が0に。
            measure(_02.Run);

            // ValueTask は名前通り、構造体
            // 完了済みのものの await なら Task クラスのインスタンスを作らないんで、ヒープ確保要らない
            // これが結構有効な場面あり

            // このために、非同期メソッドの戻り値を任意の型にできるようになった
            // 所定のパターン (async method builder)の実装が必要

            // 他の用途:
            // - WinRT の IAsyncOperation を返す
            // - Rx の何かを返す
            // - await の既定動作を ConfigureAwait(false) 相当にしてしまう
        }
    }
}
