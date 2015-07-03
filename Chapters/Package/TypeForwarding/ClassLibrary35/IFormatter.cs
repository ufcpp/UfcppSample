using System;

namespace ClassLibrary
{
    public interface IFormatter
    {
        IFormattable Format(int x, int y, int z);
    }
}
