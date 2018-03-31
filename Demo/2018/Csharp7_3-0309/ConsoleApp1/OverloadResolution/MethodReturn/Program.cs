using System;

namespace ConsoleApp1.OverloadResolution.MethodReturn
{
    class Program
    {
        // この2つみたいに、戻り値違いのデリゲートを受け取るオーバーロードがあったとき
        static void M(Func<int, int> f) => Console.WriteLine("int => int");
        static void M(Func<int, bool> f) => Console.WriteLine("int => bool");
        static void M<T>(Func<int, T> f) => Console.WriteLine("int => generic T");

        static void Main()
        {
            // ラムダ式の型推論は元から賢い。
            M(x => x); // int => int
            M(x => true); // int => bool
            M(x => x.ToString()); // int => generic T

            // メソッド グループを渡すと、これまではオーバーロード解決できなかった。
            // C# 7.3 ではちゃんと戻り値も見て候補を絞るようになった。
            M(IntToInt); // int => int
            M(IntToBool); // int => bool
            M(IntToStrint); // int => generic T

            // ちなみに、ラムダ式を介せば C# 7.2 以前でも大丈夫だった。面倒だった。
            M(x => IntToInt(x)); // int => int
            M(x => IntToBool(x)); // int => bool
            M(x => IntToStrint(x)); // int => generic T
        }

        static int IntToInt(int x) => x;
        static bool IntToBool(int x) => true;
        static string IntToStrint(int x) => x.ToString();
    }
}
