namespace StructLayoutSample.AbuseUnion
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    struct Union
    {
        [FieldOffset(0)]
        public bool Bool;

        [FieldOffset(0)] // Bool と同じ場所
        public byte Byte;
    }

    class Program
    {
        static void Main()
        {
            Write(false);   // False
            Write(true);    // True

            Write(Bool(0)); // False … false と一緒
            Write(Bool(1)); // True … true と一緒
            Write(Bool(2)); // Other!
        }

        private static bool Bool(byte value)
        {
            var union = new Union();
            union.Byte = value;
            return union.Bool;
        }

        static void Write(bool x)
        {
            switch (x)
            {
                case true:
                    Console.WriteLine("True");
                    break;
                case false:
                    Console.WriteLine("False");
                    break;
                default:
                    Console.WriteLine("Other!");
                    break;
            }
        }
    }
}
