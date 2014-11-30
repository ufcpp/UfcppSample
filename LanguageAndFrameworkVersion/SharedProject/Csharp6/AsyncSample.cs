#if Ver4 && Plus

using System;
using System.IO;
using System.Threading.Tasks;

namespace VersionSample.Csharp6
{
    /// <summary>
    /// catch 句、finally 句内での await は、
    /// async/await さえ使えるバージョンであれば必ず使える。
    /// </summary>
    public class AsyncSample
    {
        public static async Task XAsync()
        {
            try
            {
                await Csharp5.AsyncSample.XAsync();
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

        public static async Task AproxSameXAsync()
        {
            // 要は、1段階多く try で囲って catch-throw し直ししたり、一度例外を変数で受けて、null じゃなかったら処理するみたいなことしてる。

            Exception ex1 = null;
            try
            {
                Exception ex2 = null;
                try
                {
                    await Csharp5.AsyncSample.XAsync();
                }
                catch (InvalidOperationException e)
                {
                    ex2 = e;
                }

                if (ex2 != null)
                {
                    using (var s = new StreamWriter("error.txt"))
                        await s.WriteAsync(ex2.ToString());
                }
            }
            catch(Exception e1)
            {
                ex1 = e1;
            }

            using (var s = new StreamWriter("trace.txt"))
                await s.WriteAsync("XAsync done.");

            if (ex1 != null)
            {
                throw ex1;
                // ほんとは、スタックトレースを保存する処理とかがさらに挟まってる
            }
        }
    }
}

#endif
