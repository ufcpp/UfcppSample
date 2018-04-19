using System;

namespace ConsoleApp1.OverloadResolution.Refness
{
    static class Extensions
    {
        // ref あり/なし 違いのオーバーロードはできる
        // ref を使いたいのは構造体だけなので、struct/class 制約違いのオーバーロード解決との相性はよさそう。
        public static void M<T>(this ref T t)
            where T : struct
            => Console.WriteLine($"struct {t}");

        public static void M<T>(this T t)
            where T : class
            => Console.WriteLine($"class {t}");
    }

    class Program
    {
        static void Main()
        {
            // DateTime → struct 制約を満たすので、M(ref T) の方が呼ばれる
            var x = DateTime.Now;
            x.M();

            // ただし、 DateTime.Now.M(); だと呼べない。Now がプロパティなので、ref では呼べない。
            // 1.M(); でも同様。リテラルに対して ref 拡張メソッドを呼べない。
            // かといって、this in T では、ジェネリックな拡張メソッドを作れない(拡張メソッドの仕様でそうなってる)。

            // string → class 制約を満たすので、M(T) の方が呼ばれる
            "abc".M();
        }
    }
}
