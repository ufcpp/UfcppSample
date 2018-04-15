namespace OverloadResolution.ExpressionTree
{
    using System;
    using System.Linq.Expressions;

    class Program
    {
        static void Main()
        {
#if Uncompilable
            M(x => x);
#endif
        }

        static void M(Func<int, int> f) => Console.WriteLine("Func");
        static void M(Expression<Func<int, int>> f) => Console.WriteLine("Expression");
    }
}
