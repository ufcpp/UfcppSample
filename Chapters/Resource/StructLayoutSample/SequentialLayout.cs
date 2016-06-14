namespace StructLayoutSample.SequentialLayout
{
    using System;

    struct Sample
    {
        public byte A;
        public long B;
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
