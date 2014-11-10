#if Ver4Plus

using System.IO;
using System.Threading.Tasks;

namespace VersionSample.Csharp5
{
    class AsyncSample
    {
        public static async Task RunAsync()
        {
            using (var w = new StreamWriter("out.txt"))
            {
                for (int i = 0; i < 100; i++)
                {
                    await w.WriteLineAsync("line " + i);
                }
            }
        }
    }
}

#endif
