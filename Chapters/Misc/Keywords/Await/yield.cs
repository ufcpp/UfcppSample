using System.Collections.Generic;
using System.Threading.Tasks;

namespace Keywords.Await
{
    class YieldAndAwait
    {
        static IEnumerable<int> Yield()
        {
            var yield = 1; // OK
            yield return yield;
        }

        static async Task<int> Await()
        {
            //var await = 1; // これはコンパイル エラー
            await Task.Delay(1);
            return 1;
        }
    }
}
