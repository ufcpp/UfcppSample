using System;
using static System.Linq.Expressions.Expression;

namespace ConsoleApp1.Constraints.SystemDelegate
{
    class Program
    {
        // C# 7.3 で、型制約に Delegate を指定できるように。
        // Enum と同様、この Delegate は System.Delegate クラスのこと。
        // System.MulticastDelegate の指定も可能。
        static object DynamicInvoke<T>(T d, params object[] args)
            where T : Delegate
        {
            // 引数の型まで制約に付けれないから結局 DynamicInvoke…
            // パフォーマンス的には残念。
            return d.DynamicInvoke(args);
        }

        // まあ、Expression とかを使った動的コード生成では使い道あるかも。
        // 例として、任意の引数・戻り値を持つデリゲートを、キャストを挟んで Func<object, object> にする処理。
        static Func<object, object> CreateDelegate<T>(T @delegate)
            where T : Delegate
        {
            var t = typeof(T);
            var invoke = t.GetMethod("Invoke"); // Delegate の時点で Invoke を持っていることは確定。

            // とはいえやっぱり、引数まで制約を付けれないので、望まない型を受け付けてしまう可能性あり。
            // この例でいうと、引数の数を1個に限りたいけど、そこまでは保証できない。
            if (invoke.GetParameters().Length >= 2) throw new InvalidOperationException();

            var p = invoke.GetParameters()[0].ParameterType;
            var r = invoke.ReturnType;

            var x = Parameter(typeof(object));

            // (object)invoke((ParameterType)x)
            var ex = Lambda<Func<object, object>>(
                Convert(Invoke(Constant(@delegate), Convert(x, p)), typeof(object)),
                x);

            return ex.Compile();
        }

        static void Main()
        {
            Func<int, int> i = x => x * x;
            Func<string, int> s = x => x.Length;

            Console.WriteLine(DynamicInvoke(i, 3));

            Func<object, object> f1 = CreateDelegate(i);
            Func<object, object> f2 = CreateDelegate(s);

            Console.WriteLine(f1(5));
            Console.WriteLine(f2("abcde"));
        }
    }
}
