using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new ClassLibrary7.Class1();
            Console.WriteLine("Hello World!" + (x.X, x.Y));
        }
    }
}
