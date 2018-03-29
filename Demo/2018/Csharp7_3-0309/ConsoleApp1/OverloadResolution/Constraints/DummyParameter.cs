using System;

namespace ConsoleApp1.OverloadResolution.Constraints
{
    // 呼び分けのためだけの何の意味もない構造体。
    struct Struct { }
    struct Class { }

    class DummyParameter
    {
        // C# 7.3 的に呼び分けはできるのに、.NET ランタイムの制限で同じシグネチャのメソッドを作れないのであれば、
        // とりあえずダミーの引数を足して .NET ランタイムをだます。
        // default 値を与えてあるので、呼ぶ側は M(T) だけで呼べる。
        static void M<T>(T t, Struct _ = default)
            where T : struct
            => Console.WriteLine($"struct {t}");

        static void M<T>(T t, Class _ = default)
            where T : class
            => Console.WriteLine($"class {t}");

        static void Main()
        {
            // DateTime → struct 制約を満たすので、M(T, Struct) の方が呼ばれる
            M(DateTime.Now);

            // string → class 制約を満たすので、M(T, Class) の方が呼ばれる
            M("abc");
        }
    }
}
