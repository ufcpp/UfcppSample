namespace DelegateInternal.Instance
{
    using System;

    class Sample
    {
        static void StaticMethod(int x)
        {
            // 静的メソッドの場合は正真正銘、引数は x の1つだけ
        }

        void InstanceMethod(int x)
        {
            // 引数が1つだけに見えて…

            // 実は暗黙的に this を受け取っている
            Console.WriteLine(this);
        }

        // ということで ↑の InstanceMethod は、以下のような静的メソッドと同じ引数の受け取り方をしてる
        static void InstanceLikeMethod(Sample @this, int x)
        {
            Console.WriteLine(@this);
        }
    }
}

namespace DelegateInternal.Extension
{
    using System;

    class Sample
    {
        public void InstanceMethod(int x)
        {
            // 引数が1つだけに見えて、実は暗黙的に this を受け取っている
        }

        // ということで ↑の InstanceMethod は、以下のような静的メソッドと同じ引数の受け取り方をしてる
        static void InstanceLikeMethod(Sample @this, int x)
        {
        }
    }

    static class SampleExtensions
    {
        // であれば、こういう拡張メソッドも InstanceMethod と同じ引数の受け取り方になる
        public static void ExtensionMethod(this Sample @this, int x)
        {
        }
    }

    class Program
    {
        static void Main()
        {
            var x = new Sample();

            Action<int> i = x.InstanceMethod;

            // 拡張メソッドに対して、インスタンス メソッドと同じようなデリゲートの作り方を認めてる
            Action<int> e = x.ExtensionMethod;
        }
    }
}

namespace DelegateInternal.StaticVsCurriedDelegate
{
    using System;

    static class Program
    {
        // 普通の静的メソッド
        static int F(int x) => 2 * x;

        // わざわざ使いもしない第1引数を増やして、拡張メソッドに変更
        static int F(this object dummy, int x) => 2 * x;

        static void Main()
        {
            // 静的メソッドからデリゲート作成
            Func<int, int> s = F;

            // わざわざ null を使ってカリー化デリゲートを作る
            Func<int, int> e = default(object).F;

            // 以下の2つの呼び出しでは、e (カリー化デリゲート)の方が圧倒的に高速
            s(10);
            e(10);
        }
    }
}
