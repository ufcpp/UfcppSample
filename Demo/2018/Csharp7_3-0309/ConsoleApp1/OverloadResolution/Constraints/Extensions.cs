using System;

namespace ConsoleApp1.OverloadResolution.Constraints
{
    // 拡張メソッドであれば、別のクラス中に、同じ型に対する同名の拡張メソッドを定義できる。
    // これを使って、型制約だけが違う同シグネチャのメソッドを作ってしまう。

    static class ExtensionsForStruct
    {
        public static void M<T>(this T t)
            where T : struct
            => Console.WriteLine($"struct {t}");
    }

    static class ExtensionsForClass
    {
        public static void M<T>(this T t)
            where T : class
            => Console.WriteLine($"class {t}");
    }

    class Extensions
    {
        static void Main()
        {
            // DateTime → struct 制約を満たすので、ExtensionsForStruct.M(T) の方が呼ばれる
            DateTime.Now.M();

            // string → class 制約を満たすので、ExtensionsForClass.M(T) の方が呼ばれる
            "abc".M();
        }
    }
}
