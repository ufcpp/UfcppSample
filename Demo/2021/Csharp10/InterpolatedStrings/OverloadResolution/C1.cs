using System.Runtime.CompilerServices;

namespace OverloadResolution;

internal class C1
{
    public static void Caller()
    {
        // ハンドラー型最優先。
        M($"{1}"); // handler

        // ただの文字列の場合は string に行く。
        M("abc"); // string

        // ちょっと混乱しそうなのが、const になる場合に限り、 $ がついてても string 行き。
        M($""); // string
        M($"abc {"abc"} abc"); // string

        // もちろん、キャストしてしまえば任意に呼び分け可能。
        M($"{1}"); // handler
        M((string)$"{1}"); // string
        M((IFormattable)$"{1}"); // formattable
    }

    // この状態だとハンドラー型優先。
    // DefaultInterpolatedStringHandler だけ特別とかはなく、ハンドラー パターンを満たす型は一律この優先順位。
    public static void M(DefaultInterpolatedStringHandler _) => Console.WriteLine("handler");
    public static void M(string _) => Console.WriteLine("string");
    public static void M(IFormattable _) => Console.WriteLine("formattable");
}
