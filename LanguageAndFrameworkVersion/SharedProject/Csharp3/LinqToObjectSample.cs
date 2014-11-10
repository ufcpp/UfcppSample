#if Ver2Plus

using System;
using System.Linq;

namespace VersionSample.Csharp3
{
    /// <summary>
    /// LINQ to Objects は、要は <see cref="System.Collections.Generic.IEnumerable{T}"/> に対する拡張メソッドでしかないので、
    /// .NET 3.0 の <see cref="System.Linq.Enumerable"/> と全く同じものを自作してしまえば、.NET 2.0 でも LINQ to Objects が使える。
    /// <see cref="System.Linq.Enumerable"/> は実装難易度もそんなに高くないし、Mono のソースコードでも使えばコピペで済む(他のクラスへの依存も少ないのでほぼファイルのコピーで終わり)し、簡単。
    /// </summary>
    public class LinqToObjectSample
    {
        public void Run()
        {
            var input = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var output =
                from x in input
                where (x % 2) == 1
                select x * x;

            foreach (var x in output)
            {
                Console.WriteLine(x);
            }
        }
    }
}

#endif
