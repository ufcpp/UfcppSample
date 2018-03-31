using System;

namespace ConsoleApp1.OverloadResolution.Static
{
    class Program
    {
        // M(0) とか書くとどっちを呼ぶべきかわからなくなるシグネチャ。
        // 静的か、インスタンスかの差がある。
        static void M(double x) => Console.WriteLine("static");
        void M(decimal x) => Console.WriteLine("instance");

        // Constraints.Program の方で説明しているように、完全に同じシグネチャで静的/インスタンス メソッドを定義するのはいまだできない。
        // ごまかすなら以下のように弁別用のダミー引数などを利用。

        static void M(Static _ = default) => Console.WriteLine("static");
        void M(Instance _ = default) => Console.WriteLine("instance");

        static void Main()
        {
            // C# 7.2 までは、シグネチャで判定 → 静的かインスタンスかが合わなければエラーの順だったので、M(double) と M(decimal) が弁別できない。
            // C# 7.3 で、先に静的かインスタンスかで候補を絞ってからシグネチャを調べるようになったので、以下のようなコードがコンパイル可能に。
            Program.M(0); // static
            new Program().M(0); // instance

            Program.M(); // static
            new Program().M(); // instance

            // 静的メソッド内から呼んだので static void M の方が呼ばれる
            M(); // static
            new Program().InstanceMethod(); // instance
        }

        void InstanceMethod()
        {
#if Uncompilable
            // これは C# 7.3 でもエラー。
            // 型名. か this. かが付いてないとどちらか判別できない。
            M();
#endif

            // これはいける。this が付いてる時点でインスタンス。
            this.M();
        }

        // ちなみに、C# には「Color Color 問題」ってのがある。
        // C# は、Color Color { get; } みたいなプロパティ(など)を作れる。
        // 1個目の Color は型名で、2個目の Color はプロパティ名。
        // このとき、「Color.メンバー名」は静的/インスタンスどちらを呼ぶべきか、呼び分けれるべきかという問題。
        // C# は呼び分けできるようになってるんだけど、そうすると今度はオーバーロード解決で悩むことがある。
        // (「Color Color 問題」って呼ばれるのは、単に Color 構造体でよくこんなプロパティが作られるので、代表例として選ばれただけ)
        static Color Color;

        static void InvokeColor()
        {
            // 引数の型を指定すれば弁別できる(元から)。
            Color.M(default(Static)); // static の方が呼ばれる
            Color.M(default(Instance)); // instance の方が呼ばれる

#if Uncompilable
            // でも、これは C# 7.3 でも弁別できない。
            Color.M();
#endif
        }
    }

    struct Color
    {
        public static void M(Static _ = default) => Console.WriteLine("Color static");
        public void M(Instance _ = default) => Console.WriteLine("Color instance");
    }

    // 弁別用のダミー型。
    struct Static { }
    struct Instance { }
}
