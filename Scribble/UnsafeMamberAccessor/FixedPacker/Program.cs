using System;
using System.Runtime.CompilerServices;
using System.Text.Utf8;
using D = FixedPacker.Samples.DefinitionData;
using G = FixedPacker.Samples.GeneratedData;

namespace FixedPacker
{
    class Program
    {
        static void Main(string[] args)
        {
            var rawData = new[]
            {
                new D.Sample(0x11111111, 0x11, 0x1111111111111111, 0x1111, new Utf8String("abcde")),
                new D.Sample(0x22222222, 0x22, 0x2222222222222222, 0x2222, new Utf8String("あいうえ")),
                new D.Sample(0x33333333, 0x33, 0x3333333333333333, 0x3333, new Utf8String("☹☺")),
                new D.Sample(0x44444444, 0x44, 0x4444444444444444, 0x4444, new Utf8String("🐭🐮🐯")),
                new D.Sample(0x55555555, 0x55, 0x5555555555555555, 0x5555, new Utf8String("áïûèø")),
                new D.Sample(0x66666666, 0x66, 0x6666666666666666, 0x6666, new Utf8String("αβγ")),
                new D.Sample(0x77777777, 0x77, 0x7777777777777777, 0x7777, new Utf8String("aαℵあ亜🐭")),
                new D.Sample(0x08888888, 0x88, 0x0888888888888888, 0x0888, new Utf8String("БДУ")),
            };

            Test(rawData, new Packer<D.Sample, G.Sample>(G.Sample.Pack, G.Sample.GetBinaries), (p, x) => (x.Id, x.A, x.B, x.C, p.GetString(x.DIndex)).ToString());
            Test(rawData, new Packer<D.Sample, G.Sample_A_B_C>(G.Sample_A_B_C.Pack, G.Sample_A_B_C.GetBinaries), (p, x) => (x.A, x.B, x.C).ToString());
            Test(rawData, new Packer<D.Sample, G.Sample_Id_B_D>(G.Sample_Id_B_D.Pack, G.Sample_Id_B_D.GetBinaries), (p, x) => (x.Id, x.B, p.GetString(x.DIndex)).ToString());
        }

        private static void Test<TGen>(D.Sample[] rawData, Packer<D.Sample, TGen> packer, Func<Unpacker<TGen>, TGen, string> toString)
            where TGen : struct
        {
            var size = Unsafe.SizeOf<TGen>();
            var data = packer.Pack(rawData);

            var i = 0;
            foreach (var x in data)
            {
                if (i % size == 8)
                    Console.WriteLine();
                Console.Write(x.ToString("X2"));
                ++i;
            }
            Console.WriteLine();

            var u = new Unpacker<TGen>(data);

            foreach (var x in u)
            {
                Console.WriteLine(toString(u, x));
            }
        }
    }
}
