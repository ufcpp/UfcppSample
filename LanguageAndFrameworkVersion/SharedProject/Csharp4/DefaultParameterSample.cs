using System;

namespace VersionSample.Csharp4
{
    /// <summary>
    /// 省略可能パラメーター(パラメーターの規定値)、名前付きパラメーターは、IL レベルでは .NET 1.0 の頃からある。
    /// (C# 1.0 の頃でも、<see cref="System.Runtime.InteropServices.OptionalAttribute"/> を付ければ、VB からは省略可能パラメーターとして使える。)
    ///
    /// C# 的には 4.0 から入った機能だけども、上記の .NET 1.0 からある仕組みに対応しただけなので、.NET 1.0 上でも動く。
    /// (利用に多少注意が必要な機能なので、結構長い間機能追加をためらってた。)
    /// </summary>
    public class DefaultParameterSample
    {
        public static void X()
        {
            Console.WriteLine(Sum(1, 2, 3));
            Console.WriteLine(Sum(x: 1, y: 2, z: 3));
            Console.WriteLine(Sum(y: 2, z: 3));
            Console.WriteLine(Sum(x: 1, z: 3));
            Console.WriteLine(Sum(x: 1, y: 2));
            Console.WriteLine(Sum(x: 1));
            Console.WriteLine(Sum(y: 2));
            Console.WriteLine(Sum(z: 3));
            Console.WriteLine(Sum());
        }

        public static void SameAsX()
        {
            // つまるところ、定数埋め込みに展開(const と同じ)される。
            // ほんと(利用側コードへの)埋め込みなので、もしも規定値を変更した場合、利用側も再コンパイルしないと変更が反映されない。
            // (ここが問題。const も同様の問題抱えてる。この問題があるから、C# は4.0までこの機能を入れるの渋ってた。)
            Console.WriteLine(Sum(1, 2, 3));
            Console.WriteLine(Sum(1, 2, 3));
            Console.WriteLine(Sum(0, 2, 3));
            Console.WriteLine(Sum(1, 0, 3));
            Console.WriteLine(Sum(1, 2, 0));
            Console.WriteLine(Sum(1, 0, 0));
            Console.WriteLine(Sum(0, 2, 0));
            Console.WriteLine(Sum(0, 0, 3));
            Console.WriteLine(Sum(0, 0, 0));
        }

        public static int Sum(int x = 0, int y = 0, int z = 0)
        {
            return x + y + z;
        }
    }
}
