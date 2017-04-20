using System;
using System.Runtime.CompilerServices;

namespace GenericBits
{
    class Program
    {
        static void Main(string[] args)
        {
            Write((byte)0x12);
            Write((short)0x1234);
            Write(0x1234_5678);
            Write(0x12345678abcdef0L);
            Write(new Sample1(0x12, 0x34, 0x5678, 0x9abcdef0));
            Write(new Sample2(0x1234, 0x5678_9abc, 0xde, 0x1234_5678_9abc_def0L));
        }

        static void Write<T>(T initialValue)
            where T : struct
        {
            Console.WriteLine($"---- write bits for {typeof(T).Name} (size = {Unsafe.SizeOf<T>()}) ----");

            var x = initialValue;
            var bits = new Bits<T>(ref x);
            Console.WriteLine($"bits: {bits.Count}");
            Console.WriteLine(x);
            WriteBits(bits);

            for (int i = 0; i < bits.Count; i++) bits[i] = (i % 2) == 1;
            Console.WriteLine(x);
            WriteBits(bits);

            for (int i = 0; i < bits.Count; i++) bits[i] = (i % 3) == 1;
            Console.WriteLine(x);
            WriteBits(bits);
        }

        static void WriteBits<T>(Bits<T> bits)
            where T : struct
        {
            for (int i = 0; i < bits.Count; i++)
            {
                if ((i % 4 == 0 && i != 0)) Console.Write('_');
                Console.Write(bits[i] ? '1' : '0');
            }
            Console.WriteLine();
        }
    }
}