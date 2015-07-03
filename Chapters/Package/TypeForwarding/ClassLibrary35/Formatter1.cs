using System;

namespace ClassLibrary
{
    public class Formatter1 : IFormatter
    {
        public IFormattable Format(int x, int y, int z) => $"({x}, {y}, {z})";
    }
}
