using System;
using System.Collections.Generic;

namespace ConsoleApplication
{
    class Class1
    {
        public int X { get; set; }

        public int Y { get; set; }

        public override string ToString() => $"({X}, {Y})";
    }
}
