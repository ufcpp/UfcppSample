using System;
using System.Runtime.InteropServices;

namespace BoolOtherThan01
{
    class StructUnion
    {
        static void Main()
        {
            bool toBool(byte b)
            {
                Union u = default;
                u.Byte = b;
                return u.Boolean;
            }

            Console.WriteLine(toBool(2));
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct Union
        {
            [FieldOffset(0)]
            public byte Byte;
            [FieldOffset(0)]
            public bool Boolean;
        }
    }
}
