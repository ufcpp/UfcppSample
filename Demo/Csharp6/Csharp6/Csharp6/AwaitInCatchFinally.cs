using System;
using System.IO;
using System.Threading.Tasks;

namespace Csharp6.Csharp6
{
    class AwaitInCatchFinally
    {
        public static async Task XAsync()
        {
            try
            {
                await SomeAsyncMethod();
            }
            catch (InvalidOperationException e)
            {
                using (var s = new StreamWriter("error.txt"))
                    await s.WriteAsync(e.ToString());
            }
            finally
            {
                using (var s = new StreamWriter("trace.txt"))
                    await s.WriteAsync("XAsync done.");
            }
        }

        private static Task SomeAsyncMethod()
        {
            return Task.Delay(1);
        }
    }
}
