using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp6.Misc
{
    class AwaitOptimization
    {
        public static IEnumerable<int> GetXItems()
        {
            var x = 10;
            yield return x;

            var y = x * x;  // x は yield を超えて使っている
            yield return y; // y は yield を超えない

            yield return x;
        }

        public static async Task XAsync()
        {
            var x = 10;
            await Task.Delay(x);

            var y = x * x;       // x は await を超えて使っている
            await Task.Delay(y); // y は await を超えない

            await Task.Delay(x);
        }

    }
}
