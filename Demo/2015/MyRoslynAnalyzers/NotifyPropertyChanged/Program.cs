using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifyPropertyChanged
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Sample();

            s.PropertyChanged += (_, e) =>
            {
                switch (e.PropertyName)
                {
                    case "X":
                        Console.WriteLine($"X = {s.X}");
                        break;
                    case "Y":
                        Console.WriteLine($"Y = {s.Y}");
                        break;
                }
            };

            s.X = 10;
            s.Y = 20;

/*
結果:

X = 10
Y = 20
*/
        }
    }
}
