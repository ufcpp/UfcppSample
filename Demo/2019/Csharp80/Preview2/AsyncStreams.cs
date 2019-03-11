// たぶん、GetAsyncEnumerator(CancellationToken) の引数がないから。

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cs8InVs2019P1.AsyncStreams
{
    class Program
    {
        static async Task Main()
        {
            await AsyncForeach(AsyncIterator());
        }

        // 非同期イテレーター … await/yield混在
        static async IAsyncEnumerable<int> AsyncIterator()
        {
            await Task.Delay(1);
            yield return 1;
            await Task.Delay(1);
            yield return 2;
        }

        // 非同期 foreach … IAsyncEnumerable からの列挙
        static async Task AsyncForeach(IAsyncEnumerable<int> items)
        {
            await foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }
    }
}
