namespace OverloadResolution.Csharp6Lambda
{
    using System;
    using System.Linq.Expressions;

    class Program
    {
        static void Main()
        {
            // M(() => { }) だと Action か Expression<Action> か区別つかないものの
            // 匿名メソッド式の場合は式ツリー化できない仕様なので、M(Action) で確定
            // なのに以前はこれもエラーになってた(C# 6.0 からは M(Action) が呼ばれる)
            M(delegate () { });

            // 以下のような、多段のラムダ式でちゃんとオーバーロード解決できるのは C# 6.0 から
            // Func<int, Func<int>> の方
            M(() => () => 1);
            // Func<int, Func<double>> の方
            M(() => () => 1.0);
        }

        // ラムダ式だと区別できないものの、匿名メソッド式なら Action で確定
        static void M(Action x) => Console.WriteLine("Action");
        static void M(Expression<Action> x) => Console.WriteLine("Expression");

        // () => () => 1 みたいな、多段のラムダ式
        static void M(Func<Func<int>> x) => Console.WriteLine("() → () → int");
        static void M(Func<Func<double>> x) => Console.WriteLine("() → () → int");
    }
}
