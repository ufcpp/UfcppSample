using System;

namespace TupleMutableStruct.MemoryLayout.PointerAdd
{
    public struct Vector : IEquatable<Vector>
    {
        public byte A;
        public byte B;
        public byte C;
        public byte D;
        public byte E;
        public byte F;
        public byte G;
        public byte H;

        public Vector(byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h) => (A, B, C, D, E, F, G, H) = (a, b, c, d, e, f, g, h);
        public Vector((byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h) t) => (A, B, C, D, E, F, G, H) = t;
        public void Deconstruct(out byte a, out byte b, out byte c, out byte d, out byte e, out byte f, out byte g, out byte h) => (a, b, c, d, e, f, g, h) = (A, B, C, D, E, F, G, H);
        public (byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h) ToTuple() => (A, B, C, D, E, F, G, H);
        public static implicit operator (byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h) (Vector x) => x.ToTuple();
        public static implicit operator Vector((byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h) t) => new Vector(t);

        public bool Equals(Vector other) => Equals(this, other);
        public override bool Equals(object obj) => obj is Vector other && Equals(this, other);
        public override int GetHashCode() => base.GetHashCode();

        public unsafe static bool Equals(Vector x, Vector y)
        {
            var lx = *(ulong*)(&x);
            var ly = *(ulong*)(&y);
            return lx == ly;
        }

        static ulong evenMask = 0x00ff00ff00ff00ff;
        static ulong oddMask = 0xff00ff00ff00ff00;

        public unsafe static Vector operator +(Vector x, Vector y)
        {
            var lx = *(ulong*)(&x);
            var ly = *(ulong*)(&y);
            var lz = evenMask & ((evenMask & lx) + (evenMask & ly))
                | oddMask & ((oddMask & lx) + (oddMask & ly));
            return *(Vector*)(&lz);
        }

        public unsafe static Vector operator *(byte a, Vector x)
        {
            var lx = *(ulong*)(&x);
            var lz = evenMask & (a * (evenMask & lx))
                | oddMask & (a * (oddMask & lx));
            return *(Vector*)(&lz);
        }
    }
}
