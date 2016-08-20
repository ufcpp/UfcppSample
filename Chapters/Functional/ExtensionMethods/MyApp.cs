using static System.Console;
using A;

using Lib = C.Lib;
static class Lib { public static void F(this int x) => WriteLine("global"); }

namespace MyApp
{
    using B;

    static class Lib { public static void F(this int x) => WriteLine("MyApp"); }

    class Program
    {
        static void Main()
        {
            // F 拡張メソッドは5つある
            // この場合 MyApp.Lib.F が使われる
            // 優先度 高 MyApp > B > global = C > A 低
            10.F();

            // ちゃんと呼び分けたければ拡張メソッドとして使うことをあきらめる
            // 完全修飾名を使って、普通の静的メソッドとして呼ぶ
            A.Lib.F(10);
            B.Lib.F(10);
            C.Lib.F(10);
            MyApp.Lib.F(10);
            global::Lib.F(10);
        }
    }
}

namespace A
{
    static class Lib { public static void F(this int x) => WriteLine("A"); }
}
namespace B
{
    static class Lib { public static void F(this int x) => WriteLine("B"); }
}
namespace C
{
    static class Lib { public static void F(this int x) => WriteLine("C"); }
}
