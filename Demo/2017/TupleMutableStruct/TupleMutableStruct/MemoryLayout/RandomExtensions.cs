using System;

namespace TupleMutableStruct.MemoryLayout
{
    static class RandomExtensions
    {
        public static (byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h) RandomVector(this Random r) => (RandomByte(r), RandomByte(r), RandomByte(r), RandomByte(r), RandomByte(r), RandomByte(r), RandomByte(r), RandomByte(r));
        public static byte RandomByte(this Random r) => (byte)(r.Next() % 256);

    }
}
