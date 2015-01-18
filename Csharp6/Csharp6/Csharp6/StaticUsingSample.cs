namespace Csharp6.Csharp6.StaticUsing
{
    using System;
    using static System.Math;

    class Program
    {
        static void Main()
        {
            var pi = 2 * Asin(1);
            Console.WriteLine(PI == pi);
        }
    }
}

#if false
// もしも、using static に「static」が必要なかったら(名前空間に対する using との区別がつかなかったら)どうなるか。
// 実際、プレビュー版の頃の C# 6.0 では using だけ(名前空間と同じ文法)だったので問題があった。

namespace Csharp6.Csharp6.StaticUsingIf
{
    using System;
    using System.Linq;

    // ↑ 静的クラスの方の Linq が参照される。
    // 本来の LINQ (System.Linq.Enumerable クラス内の拡張メソッド)は呼べなくなるわ、
    // nameof の意味が下記の Linq クラスの nameof 静的メソッドで上書きされてしまうわ、
    // ろくなことにならない。

    public class Program
    {
        public static void Main()
        {
            var name = nameof(Main); // 下記の System.Linq クラスの nameof 静的メソッドが呼ばれる。
            Console.WriteLine(name);
        }
    }

    namespace System
    {
        public static class Linq
        {
            public static string nameof(Action x) => "";
        }
    }
}

#endif
