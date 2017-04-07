using DataLib;
using LogicLib;
using System;

namespace ClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var u = new Class1User(1);
            Console.WriteLine(u.Id);

            var c = new Class1 { Id = 2 };
            Console.WriteLine(c.Id);
        }
    }
}
