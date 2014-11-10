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
        public static void Run()
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

        public static int Sum(int x = 0, int y = 0, int z = 0)
        {
            return x + y + z;
        }
    }
}
