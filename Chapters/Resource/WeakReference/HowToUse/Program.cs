namespace WeakReferenceSample.HowToUse
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        static void Main()
        {
            RunAsync().Wait();
        }

        private static async Task RunAsync()
        {
            var obj = (object)123;
            var t = StartLoop(new WeakReference<object>(obj));

            // 2.5秒後にオブジェクトを消す
            await Task.Delay(2500);
            obj = null!;
            GC.Collect();

            await t;
        }

        // 1秒に1回、「参照中」メッセージを表示
        static async Task StartLoop(WeakReference<object> r)
        {
            while (true)
            {
                if (r.TryGetTarget(out var obj))
                {
                    Console.WriteLine(obj + " を参照中");
                }
                else
                {
                    Console.WriteLine("参照がなくなりました");
                    break;
                }

                await Task.Delay(1000);
            }
        }
    }
}
