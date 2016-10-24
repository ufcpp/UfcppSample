namespace IdentifierScope.Scope
{
    using System;

    public class Program
    {
        static int _count;

        public static void M()
        {
            int x = 10;
            _count = x;
            Console.WriteLine(x);
        }
    }
}
