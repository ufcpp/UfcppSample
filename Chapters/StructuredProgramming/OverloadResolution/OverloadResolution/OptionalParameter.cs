namespace OverloadResolution.OptionalParameter
{
    using System;

    class Program
    {
        static void Main()
        {
            M();
        }

        // これが最優先
        static void M() => Console.WriteLine("void");

        // 次がこれ。既定値を与えたもの
        static void M(int x = 0) => Console.WriteLine("int x = 0");

        // 最後がこれ。params
        static void M(params int[] x) => Console.WriteLine("params int[]");
    }
}
