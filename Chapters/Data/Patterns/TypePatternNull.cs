namespace Patterns.TypePatternNull
{
    using System;

    class Program
    {
        static void Main()
        {
            M("abc"); // matched abc
            M(null);  // 何も表示されない
        }

        static void M(string x)
        {
            if (x is string s) Console.WriteLine("matched " + s);
        }

        static void M<T>(T x)
        {
            Console.WriteLine(x is null);
        }
    }
}
