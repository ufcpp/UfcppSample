using System;

namespace TupleMutableStruct.MemoryLayout.SafeAdd
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

        public static bool Equals(Vector x, Vector y)
            => x.A == y.A
            && x.B == y.B
            && x.C == y.C
            && x.D == y.D
            && x.E == y.E
            && x.F == y.F
            && x.G == y.G
            && x.H == y.H;

        public static Vector operator +(Vector x, Vector y)
            => new Vector(
                (byte)(x.A + y.A),
                (byte)(x.B + y.B),
                (byte)(x.C + y.C),
                (byte)(x.D + y.D),
                (byte)(x.E + y.E),
                (byte)(x.F + y.F),
                (byte)(x.G + y.G),
                (byte)(x.H + y.H));

        public static Vector operator *(byte a, Vector x)
            => new Vector(
                (byte)(a * x.A),
                (byte)(a * x.B),
                (byte)(a * x.C),
                (byte)(a * x.D),
                (byte)(a * x.E),
                (byte)(a * x.F),
                (byte)(a * x.G),
                (byte)(a * x.H));
    }
}
