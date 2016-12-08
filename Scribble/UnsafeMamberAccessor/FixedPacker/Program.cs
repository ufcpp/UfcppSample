using System;
using System.Text.Utf8;

namespace FixedPacker
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = Samples.Packer.Pack(new[]
            {
                new Samples.DefinitionData.Sample(0x11111111, 0x11, 0x1111111111111111, 0x1111, new Utf8String("abcde")),
                new Samples.DefinitionData.Sample(0x22222222, 0x22, 0x2222222222222222, 0x2222, new Utf8String("あいうえ")),
                new Samples.DefinitionData.Sample(0x33333333, 0x33, 0x3333333333333333, 0x3333, new Utf8String("☹☺")),
                new Samples.DefinitionData.Sample(0x44444444, 0x44, 0x4444444444444444, 0x4444, new Utf8String("🐭🐮🐯")),
                new Samples.DefinitionData.Sample(0x55555555, 0x55, 0x5555555555555555, 0x5555, new Utf8String("áïûèø")),
                new Samples.DefinitionData.Sample(0x66666666, 0x66, 0x6666666666666666, 0x6666, new Utf8String("αβγ")),
            });

            var i = 0;
            foreach (var x in data)
            {
                if (i % 24 == 8)
                    Console.WriteLine();
                Console.Write(x.ToString("X2"));
                ++i;
            }
            Console.WriteLine();

            var u = new Unpacker<Samples.GeneratedData.Sample>(data);

            foreach (var x in u)
            {
                Console.WriteLine($"{x.Id.ToString("X8")} {x.A.ToString("X2")} {x.B.ToString("X16")}, {x.C.ToString("X4")}, {u.GetString(x.DIndex)}");
            }
        }
    }
}
