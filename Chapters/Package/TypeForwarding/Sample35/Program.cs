using System;
using ClassLibrary;

namespace Sample35
{
    class Program
    {
        static void Main(string[] args)
        {
            Format(new Formatter1());
        }

        static void Format(IFormatter f) => Console.WriteLine(f.Format(1, 2, 3));
    }
}
