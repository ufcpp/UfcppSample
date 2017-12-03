namespace MakeRef
{
    using System;

    /// <summary>
    /// CppReference.vcxproj のやつと同様のコード。
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            int x = 10;
            TypedReference r = __makeref(x); // x の参照を作る

            __refvalue(r, int) = 99; // 参照元の x も書き換わる

            Console.WriteLine(x); // 99
        }
    }
}
