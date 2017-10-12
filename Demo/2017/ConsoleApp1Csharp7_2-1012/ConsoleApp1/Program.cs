using System;

namespace ConsoleApp1
{
    class Program
    {
#if たぶんリリース版までには入るであろう物

        static void RefLocalReassignment()
        {
            var x = 1;
            ref var r = ref x;

            var y = 2;
            if (x < y) ref r = ref y;

            r = 3;
        }

        static void BugFix()
        {
            void X(int? x = default) => Console.WriteLine(x);

            X();        // C# 7.0 なぜか 0 になる。7.2 で直る。null に
            X(default); // こっちは null
        }

#endif
    }
}
