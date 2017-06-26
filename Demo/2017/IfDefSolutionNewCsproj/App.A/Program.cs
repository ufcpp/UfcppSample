using System;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = LibDependentOnA.Class1Factory.Create(1);
            Console.WriteLine(c.Id);
        }
    }
}
