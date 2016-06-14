namespace StructLayoutSample.ExplicitLayout
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    struct Sample
    {
        [FieldOffset(1)]
        public byte A;
        [FieldOffset(4)]
        public long B;
        [FieldOffset(15)]
        public byte C;
    }

    class Program
    {
        static unsafe void Main()
        {
            var a = default(Sample);
            var p = &a;
            var pa = &a.A;
            var pb = &a.B;
            var pc = &a.C;

            Console.WriteLine($@"サイズ: {sizeof(Sample)}
A: {(long)pa - (long)p}
B: {(long)pb - (long)p}
C: {(long)pc - (long)p}
");
        }
    }
}
