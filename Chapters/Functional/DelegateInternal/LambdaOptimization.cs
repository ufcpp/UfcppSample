namespace DelegateInternal.LambdaOptimization
{
    using System;

    class Program
    {
        // Func 越しに何かのインスタンスを取りたい
        static void M(Func<string> factory)
        {
            Console.WriteLine(factory());
        }

        static void Main()
        {
            // でも、呼ぶ側としては単に何かインスタンスを1個渡したいだけ
            string s = Console.ReadLine();

            // そこで、ラムダ式で1段覆って、string から Func<string> を作る
            // これだと、匿名関数の仕様から、匿名のクラスが作られて、その new のコストが余計にかかる
            M(() => s);

            // 一方で、以下のように、拡張メソッドを介することで、カリー化デリゲート(速い)になる
            M(s.Identity);
        }
    }

    static class TrickyExtension
    {
        // 素通しするだけの拡張メソッドを用意
        public static T Identity<T>(this T x) => x;
    }
}
