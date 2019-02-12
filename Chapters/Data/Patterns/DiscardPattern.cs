namespace Patterns.DiscardPattern
{
    class Program
    {
        static void Main()
        {

        }

        static int M(object x)
            => x switch
            {
                0 => 0,
                string s => s.Length,
                _ => -1
            };
    }
}

namespace Patterns.DiscardPattern.BreakingChange
{
    using System;

    class _Type
    {
        class _ { }

        static void M(object x)
        {
            Console.WriteLine(x is _); // class _ とのマッチ
        }
    }

    class _Constant
    {
        const int _ = 0;

        static void M(object x)
        {
            switch (x)
            {
                case _: // 定数 _ とのマッチ
                    break;
            }
        }
    }
}
