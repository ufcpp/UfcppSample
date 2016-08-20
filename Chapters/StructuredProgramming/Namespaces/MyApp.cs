using static System.Console;
using A;

// using よりは、直接定義されているものの方が優先 A < C, global
// エイリアスと型定義は同列 C = global
using Lib = C.Lib;
class Lib { public static void F() => WriteLine("global"); }

namespace MyApp
{
    using B; // 内側に using を書くと、外より優先 A, C, global < B

    // 同一名前空間内にあるものは1番高い優先度 B < MyApp
    class Lib { public static void F() => WriteLine("MyApp"); }

    class Program
    {
        static void Main()
        {
            // Lib は5つある
            // この場合 MyApp.Lib が使われる
            // 優先度 高 MyApp > B > global = C > A 低
            Lib.F();

            // ちゃんと呼び分けたければフルネームで書く
            A.Lib.F();
            B.Lib.F();
            C.Lib.F();
            MyApp.Lib.F();
            global::Lib.F();
        }
    }
}

namespace A
{
    class Lib { public static void F() => WriteLine("A"); }
}
namespace B
{
    class Lib { public static void F() => WriteLine("B"); }
}
namespace C
{
    class Lib { public static void F() => WriteLine("C"); }
}
