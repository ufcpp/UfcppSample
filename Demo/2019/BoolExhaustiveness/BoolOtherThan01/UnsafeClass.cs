using System;
using System.Runtime.CompilerServices;

namespace BoolOtherThan01
{
    class UnsafeClass
    {
        static void Main()
        {
            bool toBool(byte b) => Unsafe.As<byte, bool>(ref b);
            Console.WriteLine(toBool(2));
        }
    }
}
