using System;

namespace ClassLibrary
{
    public class Formatter2 : IFormatter
    {
        public IFormattable Format(int x, int y, int z) => $"{x} / {y} / {z}";
    }
}
