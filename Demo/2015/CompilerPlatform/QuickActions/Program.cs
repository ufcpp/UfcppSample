using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickActions
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = 0;
            var y = 0;
            var p = new Point();
            Console.WriteLine(p);
        }
    }

    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Area => this.X * this.Y;
        public override string ToString() => $"({X}, {Y})";
    }
}
