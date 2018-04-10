using System;

namespace ConsoleApp1.OverloadResolution.Constraints
{
    class Program
    {
        // C# 7.2 までは、ジェネリック型制約(where 句の条件)はオーバーロード解決の役に立ってなかった。
        // なので、以下の M(T, double) と M(T, decimal) の呼び分けが難しい。
        static void M<T>(T t, double _)
            where T : struct
            => Console.WriteLine($"struct {t}");

        static void M<T>(T t, decimal _)
            where T : class
            => Console.WriteLine($"class {t}");

        // インターフェイス制約も同様
        static void M<T>(T x, string _) where T : IDisposable => Console.WriteLine("IDisposable");
        static void M<T>(T x, object _) where T : IComparable => Console.WriteLine("IComparable");

        struct Disposable : IDisposable { public void Dispose() { } }
        struct Comparable : IComparable { public int CompareTo(object obj) => 0; }

        static void Main()
        {
            // int → double, int → decimal のどちらを取ればいいのかわからないので、これまではエラーになってた。
            // C# 7.3 で、第1引数の where 句を見てオーバーロード解決できるようになったので、この呼び分けが可能に。
            // 動作原理としては、「引数の型変換などを調べるよりも先に、制約を見て候補を絞る」って処理を足したみたい。

            // DateTime → struct 制約を満たすので、M(T, double) の方が呼ばれる
            M(DateTime.Now, 0);

            // string → class 制約を満たすので、M(T, decimal) の方が呼ばれる
            M("abc", 0);

            // M(T, string) が呼ばれる。
            M(new Disposable(), null);

            // M(T, object) の方に行く。
            M(new Comparable(), null);
        }

#if Uncompilable
        // 残念ながら、以下のように、where 句だけが違うオーバーロードは作れない。

        static void M<T>(T t)
            where T : struct
            => Console.WriteLine($"struct {t}");

        static void M<T>(T t)
            where T : class
            => Console.WriteLine($"class {t}");


        // 上記のような呼び分けができるようになったんだから、C# としてはこのオーバーロードを作れてもいいはずなんだけども。
        // .NET の型システム的に、where 句だけが違うメソッドは定義できなくなってる。
        // これを認めるためには .NET ランタイム自体の仕様変更が必要(大変)。

        // ただ、ちょっとしたトリックで問題の回避ができなくはない。
        // 参考: DummyParameter, Extensions
        // 実用途例: FirstOrNull
#endif
    }
}
