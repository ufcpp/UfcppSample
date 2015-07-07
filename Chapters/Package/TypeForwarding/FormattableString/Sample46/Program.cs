using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;

namespace Sample46
{
    class Program
    {
        static void Main(string[] args)
        {
            Format(new Formatter1());
            Format(new Formatter2());
        }

        static void Format(IFormatter f) => Console.WriteLine(f.Format(1, 2, 3));
    }
}
