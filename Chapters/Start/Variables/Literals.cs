namespace Variables.Literals
{
    using System;

    class Program
    {
        static void M()
        {
            byte bitMask = 0b1100_0000;
            Console.WriteLine(bitMask); // 192

            uint magicNumber = 0xDEAD_BEEF;
            Console.WriteLine(magicNumber); // 3735928559
            Console.WriteLine(magicNumber.ToString("X")); // DEADBEEF 
        }
    }
}
