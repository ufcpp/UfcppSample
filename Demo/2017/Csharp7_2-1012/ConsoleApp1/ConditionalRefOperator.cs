using System;

namespace ConditionalRefOperator
{
    class Program
    {
        static void Main()
        {
            var x = 10;
            var y = 5;
            Max(ref x, ref y) = 0;

            Console.WriteLine((x, y)); // (0, 5)
        }

        // C# 7.0
        static ref int MaxCs70(ref int x, ref int y)
        {
            if (x < y) return ref y;
            else return ref x;
        }

        // C# 7.2
        static ref int Max(ref int x, ref int y) => ref x < y ? ref y : ref x;
    }
}
