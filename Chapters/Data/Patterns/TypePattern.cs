namespace Patterns.TypePattern
{
    using System;

    class Program
    {
        static void Main()
        {
            M(1);
            M("abc");
            M(new object());
        }

        static void M(object x)
        {
            if (x is int i) Console.WriteLine("int " + i);
            else if (x is string s) Console.WriteLine("string " + s);
        }

        static void M1(object x)
        {
            if (x is int)
            {
                var i = (int)x;
                Console.WriteLine("int " + i);
            }
            else
            {
                string s = x as string;
                if (s != null)
                {
                    Console.WriteLine("string " + s);
                }
            }
        }
    }
}
