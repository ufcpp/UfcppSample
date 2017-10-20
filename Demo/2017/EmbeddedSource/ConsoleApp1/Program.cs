using A;
using System;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new[] { 1, 2, 3, 4, 5 };
            var a = x.Double().ToArray();
            var b = x.Halve().ToArray();
            var c = x.Quarter().ToArray();
            var d = x.Triple().ToArray();

            Console.WriteLine(string.Join(", ", x));
            Console.WriteLine(string.Join(", ", a));
            Console.WriteLine(string.Join(", ", b));
            Console.WriteLine(string.Join(", ", c));
            Console.WriteLine(string.Join(", ", d));

            Console.WriteLine(ClassLibrary.Class1.Id);
        }
    }
}
