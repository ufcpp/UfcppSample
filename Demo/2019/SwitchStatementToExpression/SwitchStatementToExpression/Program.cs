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
        {
            switch(x)
            {
                case true: return 1;
                case false: return 0;
            }
        }
    }
}
