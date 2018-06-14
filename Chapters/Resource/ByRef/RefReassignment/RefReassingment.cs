namespace ByRef.RefReassignment
{
    using System;

    class Program
    {
        static void Main()
        {
            int x = 1;
            int y = 2;

            // x を参照。
            ref var r = ref x;

            // このとき、r に対する代入は x に反映される。
            r = 10; // x が 10 になる。

            // これが ref 再代入。
            // r が y を参照するようになる。
            r = ref y;

            // 今度は、r に対する代入が y に反映される。
            r = 20; // y が 20 になる。

            Console.WriteLine((x, y)); // (10, 20)
        }

        static void M1(ref int x, ref int y)
        {
            x = ref y;
        }

        static void M2(in int x, ref int y)
        {
            x = ref y;
            // y = ref x; ←逆は当然ダメ
        }

        static void M3(ref int x, out int y)
        {
            y = 0; // 先に値を与えないとダメ
            x = ref y;
            y = ref x;
        }
    }
}
