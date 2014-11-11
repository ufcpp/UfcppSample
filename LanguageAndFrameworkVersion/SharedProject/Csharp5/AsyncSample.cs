#if Ver4Plus

using System.IO;
using System.Threading.Tasks;

namespace VersionSample.Csharp5
{
    /// <summary>
    /// async/await は、原理的にはどんな型であも await 可能(awaitable)にできるんだけども、実際のところ、自作するのはかなり大変。
    /// まず無理。
    /// なので、素の状態では、C# 5.0 と同時期に出た .NET 4.5 以上でないと async/await を使えない。
    ///
    /// ただし、.NET 4 上で await が使えるように、.NET 4.5 相当の <see cref="System.Threading.Tasks.Task"/> クラス機能や、IO 系クラスの非同期メソッドなどを .NET 4 向けにバックポーティングしたものを、Microsoft が公式に提供してる。
    /// (NuGet パッケージとして参照可能。Microsoft.Bcl.Async。)
    /// これを使えば、.NET 4 以上であれば async/await を使える。
    /// </summary>
    public class AsyncSample
    {
        public static async Task XAsync()
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
