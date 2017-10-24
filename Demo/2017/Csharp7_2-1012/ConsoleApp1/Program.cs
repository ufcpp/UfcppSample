using System;

namespace ConsoleApp1
{
    class Program
    {
#if false // 7.2 で入るかと思っていたら 7.X 行きになってた。それで確定だったらこのコード消す

        static void RefLocalReassignment()
        {
            var x = 1;
            ref var r = ref x;

            var y = 2;
            if (x < y) ref r = ref y;

            r = 3;
        }
#endif

        static void BugFix()
        {
            void X(int? x = default) => Console.WriteLine(x);

            X();        // C# 7.1 まではなぜか 0 になってた
            X(default); // こっちは null

            // さすがにひどいバグなので、破壊的変更とはいえ、ちゃんと X() で null になるように修正された
        }
    }
}
