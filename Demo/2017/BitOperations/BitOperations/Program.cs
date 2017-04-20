using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace BitOperations
{
    class Program
    {
        static void Main(string[] args)
        {
            ByteSample();
            IntSample();
            VectorSample();
            GenericSample();
        }

        static void ByteSample()
        {
            byte b = 0;
            var bits = Bits.Create(ref b);
            Console.WriteLine(b); // 0
            WriteAsEnumerable(bits);
            WriteAsList(bits);

            b = 0b1100_1010;
            Console.WriteLine(b); // 202
            WriteAsEnumerable(bits); // 0101_0011
            WriteAsList(bits);

            for (int i = 0; i < 8; i++) bits[i] = i % 2 == 1;
            Console.WriteLine(b); // 170
            WriteAsEnumerable(bits);
            WriteAsList(bits);

            for (int i = 0; i < 8; i++) bits[i] = true;
            Console.WriteLine(b); // 255
            WriteAsEnumerable(bits);
            WriteAsList(bits);

            for (int i = 0; i < 8; i++) bits[i] = i % 2 == 0;
            Console.WriteLine(b); // 85
            WriteAsEnumerable(bits);
            WriteAsList(bits);

            for (int i = 0; i < 8; i++) bits[i] = false;
            Console.WriteLine(b); // 0
            WriteAsEnumerable(bits);
            WriteAsList(bits);
        }

        static void IntSample()
        {
            ushort i16 = 0x1234;
            var bits16 = Bits.Create(ref i16);
            WriteAsList(bits16);
            for (int i = 0; i < bits16.Count; i++) bits16[i] = i % 2 == 0;
            WriteAsList(bits16);
            Console.WriteLine(i16);

            uint i32 = 0x12345678;
            var bits32 = Bits.Create(ref i32);
            WriteAsList(bits32);
            for (int i = 0; i < bits32.Count; i++) bits32[i] = i % 2 == 0;
            WriteAsList(bits32);
            Console.WriteLine(i32);

            ulong i64 = 0x123456789ABCDEF0UL;
            var bits64 = Bits.Create(ref i64);
            WriteAsList(bits64);
            for (int i = 0; i < bits64.Count; i++) bits64[i] = i % 2 == 0;
            WriteAsList(bits64);
            Console.WriteLine(i64);
        }

        static void VectorSample()
        {
            // System.Numerics.Vector<byte> is a 16-byte struct.
            var vector = new Vector<byte>(Enumerable.Range(1, 16).Select(i => (byte)i).ToArray());

            // Vector<byte> → Bytes16 (conversion between same-size structs)
            ref Bytes16 bytes = ref Unsafe.As<Vector<byte>, Bytes16>(ref vector);

            // bit operations on Vector<byte>
            var bits = Bits.Create(ref bytes);

            WriteAsEnumerable(bits);
            WriteAsList(bits);

            for (int i = 0; i < 128; i++) bits[i] = i % 2 == 1;

            Console.WriteLine(vector);
            WriteAsEnumerable(bits);
            WriteAsList(bits);
        }

        static void GenericSample()
        {
            var vector = new Vector<byte>();
            GenericSample(vector);

            GenericSample((byte)0x12);
            GenericSample((short)0x1224);
            GenericSample(0x12345678);
            GenericSample(0x123456789ABCDEF0L);
        }

        private static void GenericSample<T>(T x)
            where T : struct
        {
            var bits = Bits.Create(ref x);
            for (int i = 0; i < bits.Count; i++) bits[i] = i % 2 == 1;

            Console.WriteLine(x);
            WriteAsEnumerable(bits);
            WriteAsList(bits);
        }

        static void WriteAsEnumerable(IEnumerable<bool> items)
        {
            int i = 0;
            foreach (var item in items)
            {
                if ((i % 4) == 0 && i != 0) Console.Write("_");
                Console.Write(item ? 1 : 0);
                i++;
            }
            Console.WriteLine();
        }

        static void WriteAsList(IReadOnlyList<bool> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if ((i % 4) == 0 && i != 0) Console.Write("_");
                Console.Write(items[i] ? 1 : 0);
            }
            Console.WriteLine();
        }
    }
}