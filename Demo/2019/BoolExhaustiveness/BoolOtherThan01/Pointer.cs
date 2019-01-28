namespace BoolOtherThan01
{
    using System;

    class Pointer
    {
        static void Main()
        {
            unsafe bool toBool(byte b) => *((bool*)&b);

            Branch(false);     // if (false)
            Branch(true);      // if (true)
            Branch(toBool(2)); // if (true)
        }

        static void Branch(bool b)
        {
            if (b) Console.WriteLine("if (true)");
            else Console.WriteLine("if (false)");
        }
    }
}
