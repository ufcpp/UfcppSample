namespace StructLayoutSample.Union
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    struct Union
    {
        [FieldOffset(0)]
        public byte A;

        [FieldOffset(1)]
        public byte B;

        [FieldOffset(2)]
        public byte C;

        [FieldOffset(3)]
        public byte D;

        [FieldOffset(0)] // A と一緒
        public int N;
    }

    class Program
    {
        static void Main()
        {
            var x = new Union { N = 0x12345678 };
            Console.WriteLine(x.A.ToString("x")); // 78
            Console.WriteLine(x.B.ToString("x")); // 56
            Console.WriteLine(x.C.ToString("x")); // 34
            Console.WriteLine(x.D.ToString("x")); // 12
        }
    }
}
