namespace StructLayoutSample.SequentialLayout.Pack
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    struct Pack8
    {
        public byte A;
        public long B;
        public byte C;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    struct Pack4
    {
        public byte A;
        public long B;
        public byte C;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    struct Pack2
    {
        public byte A;
        public long B;
        public byte C;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct Pack1
    {
        public byte A;
        public long B;
        public byte C;
    }

    class Program
    {
        static unsafe void Main()
        {
            Main8();
            Main4();
            Main2();
            Main1();
        }
        static unsafe void Main8()
        {
            var a = default(Pack8);
            var p = &a;
            var pa = &a.A;
            var pb = &a.B;
            var pc = &a.C;

            Console.WriteLine($@"サイズ: {sizeof(Pack8)}
A: {(long)pa - (long)p}
B: {(long)pb - (long)p}
C: {(long)pc - (long)p}
");
        }
        static unsafe void Main4()
        {
            var a = default(Pack4);
            var p = &a;
            var pa = &a.A;
            var pb = &a.B;
            var pc = &a.C;

            Console.WriteLine($@"サイズ: {sizeof(Pack4)}
A: {(long)pa - (long)p}
B: {(long)pb - (long)p}
C: {(long)pc - (long)p}
");
        }
        static unsafe void Main2()
        {
            var a = default(Pack2);
            var p = &a;
            var pa = &a.A;
            var pb = &a.B;
            var pc = &a.C;

            Console.WriteLine($@"サイズ: {sizeof(Pack2)}
A: {(long)pa - (long)p}
B: {(long)pb - (long)p}
C: {(long)pc - (long)p}
");
        }
        static unsafe void Main1()
        {
            var a = default(Pack1);
            var p = &a;
            var pa = &a.A;
            var pb = &a.B;
            var pc = &a.C;

            Console.WriteLine($@"サイズ: {sizeof(Pack1)}
A: {(long)pa - (long)p}
B: {(long)pb - (long)p}
C: {(long)pc - (long)p}
");
        }
    }
}
