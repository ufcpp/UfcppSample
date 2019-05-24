using System;

namespace SwitchStatementToExpression
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine(M(true));
            Console.WriteLine(M(false));
        }

        static int M(bool x)
            => x switch
            {
                true => 1,
                false => 0,
            };
    }
}
