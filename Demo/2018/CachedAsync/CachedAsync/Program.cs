using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CachedAsync
{
    class Program
    {
        static async Task Main()
        {
            var x = new TimerLoop(300);
            await ReadAsync(x);
            await x.StopAsync();
        }

        static async Task ReadAsync(TimerLoop x)
        {
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 10; i++)
            {
                var res = await x.XAsync();

                var e = Math.Round(sw.ElapsedMilliseconds / 10.0) / 100;
                Console.WriteLine((res, e, Thread.CurrentThread.ManagedThreadId));
            }
            sw.Stop();
        }
    }
}
