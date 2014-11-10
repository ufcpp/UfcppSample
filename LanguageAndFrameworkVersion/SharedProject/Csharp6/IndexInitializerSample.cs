using System.Collections.Generic;

namespace VersionSample.Csharp6
{
    /// <summary>
    /// オブジェクト初期化子の亜種で、オブジェクト初期化子同様、かなり単純な構文糖衣。
    /// .NET 2.0 上で余裕で動く。
    /// </summary>
    public class IndexInitializerSample
    {
        public static void Run()
        {
            var dic = new Dictionary<string, int>
            {
                ["one"] = 1,
                ["two"] = 2,
            };

            var theSameAsAbove = new Dictionary<string, int>();
            theSameAsAbove["one"] = 1;
            theSameAsAbove["two"] = 2;
        }
    }
}
