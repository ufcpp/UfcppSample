using System;

namespace ConsoleApp1.Constraints.SystemEnum
{
    class Program
    {
        // 型制約に Enum を付けれるようになった。
        // この Enum は System.Enum クラス(C# に Enum キーワードが入ったとかではないし。)
        // 今のところ、「where T : enum」 (キーワードの enum)とかは書けない。
        // (ちなみに、struct 制約と同時に付けれる。他に struct と一緒に指定できるのはインターフェイスだけ。)
        static void EnumConstraint<T>(T x, T y)
            where T : struct, Enum
        {
            // Enum 制約により、HasFlag を使えるように。
            // HasFlag は、.NET Core 2.0 まではパフォーマンス的な問題もあったけど、2.1 で最適化されるらしい:
            // https://gist.github.com/ufcpp/79283511d2a10afb34f8c5c837dce1a6
            Console.WriteLine(x.HasFlag(y));

#if Uncompilable
            // ちなみに、 == はダメ。Enum クラスに == はない。
            Console.WriteLine(x == y);
#endif
        }

        enum X
        {
            A = 1, B = 2, C = 4,
        }

        enum Y
        {
            α = 1, β = 2, γ = 4,
        }

        static void Main()
        {
            EnumConstraint(X.B, X.A);
            EnumConstraint(X.A | X.B, X.A);
            EnumConstraint(X.A | X.C, X.B);

            EnumConstraint(Y.β, Y.α);
            EnumConstraint(Y.α | Y.β, Y.α);
            EnumConstraint(Y.α | Y.γ, Y.β);

#if Uncompilable
            // 6 (もちろん int) が Enum 制約違反なのでエラーになる
            EnumConstraint(6);
#endif
        }
    }
}
