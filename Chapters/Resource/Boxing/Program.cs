using System;
using System.Reflection;

namespace Boxing
{
    class Program
    {
        static void Main(string[] args)
        {
            // サンプル掲載用に、Version* フォルダー以下の Program も単体で動くように作ってあって、
            // そっちにも private な Main メソッドがある。
            // ・ビルド設定で「スタートアップ オブジェクト」をこっちのクラスに設定してある。
            // ・あっちの private Main はリフレクションで無理やり呼ぶ。

            InvokeMain(typeof(IntAsObject.Program));
            InvokeMain(typeof(ToString.Program));
            InvokeMain(typeof(Generics.Program));
        }

        static void InvokeMain(Type t)
        {
            t.GetMethod(nameof(Main), BindingFlags.Static | BindingFlags.NonPublic)!.Invoke(null, null);
        }

        static void X1()
        {
            int x = 5;
            Console.WriteLine(x.ToString());
            Console.WriteLine(x.GetHashCode());
            Console.WriteLine(x.Equals(5));
            Console.WriteLine(x.GetType().Name);
        }

        static void X2()
        {
            int x = 5;
            object y = x;   // int を object に。ボックス化が起きる。
            int z = (int)y; // object から元の型に。ボックス化解除。

            Console.WriteLine(x);
            Console.WriteLine(y);
            Console.WriteLine(z);
        }
    }
}
