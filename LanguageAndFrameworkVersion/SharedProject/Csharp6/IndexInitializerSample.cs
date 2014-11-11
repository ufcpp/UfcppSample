using System.Collections.Generic;

namespace VersionSample.Csharp6
{
    /// <summary>
    /// オブジェクト初期化子の亜種で、オブジェクト初期化子同様、かなり単純な構文糖衣。
    /// .NET 2.0 上で余裕で動く。
    /// </summary>
    public class IndexInitializerSample
    {
        public static void X()
        {
            var dic = new Dictionary<string, int>
            {
                ["one"] = 1,
                ["two"] = 2,
            };
        }

        public static void SameAsX()
        {
            // 展開方法も、オブジェクト初期化子と似たようなもの。
            // 単に、代入がワンセットの式で書けるだけ。
            var dic = new Dictionary<string, int>();
            dic["one"] = 1;
            dic["two"] = 2;
        }
    }
}
