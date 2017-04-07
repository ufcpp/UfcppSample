using DataLib;
using LogicLib;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            var u = new Class1User(1);
            Console.WriteLine(u.Id);

            var c = new Class1();
            c.PropertyChanged += (sender, args) =>
            {
                Console.WriteLine($"{args.PropertyName} changed. {c.Id}, {c.Secret}");
            };
            c.Id = 3;
            c.Secret = "some admin data";
        }
    }
}
