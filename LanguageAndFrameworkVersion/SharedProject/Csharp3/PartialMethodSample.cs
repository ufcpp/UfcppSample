using System;

namespace VersionSample.Csharp3
{
    /// <summary>
    /// 部分メソッドは、C# 2.0 以前で同じようなコードを書く手段はないけども、単純にコンパイラーの提供する機能で、ライブラリ非依存。
    /// .NET 2.0 以前でも動く。
    /// </summary>
    public partial class PartialMethodSample
    {
        public static void X()
        {
            OnBeginX("call iif the OnBeginX body is defined in anather part of the class");

            Console.WriteLine("X body");

            OnEndX("call iif the OnEdnX body is defined in anather part of the class");
        }

        // 部分メソッド
        // この状態だけだと呼び出されない。というか、引数の評価すらしない。
        // 普通は機械生成のコードの中で宣言する。
        static partial void OnBeginX(string message);
        static partial void OnEndX(string message);
    }
}

namespace VersionSample.Csharp3
{
    public partial class PartialMethodSample
    {
        // こういう風に、別の部分クラス定義の中で部分メソッドの本体定義があったら、そこで初めてこのメソッドが呼ばれるようになる。
        // 機械生成のコードに対して、手書きで何らかの処理を挟みたい場合に重宝する。
        static partial void OnBeginX(string message)
        {
            Console.WriteLine(message);
        }

        static partial void OnEndX(string message)
        {
            Console.WriteLine(message);
        }
    }
}
