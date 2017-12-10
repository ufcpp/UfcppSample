namespace ClassLibrary1
{
    public struct DifferentAssembly
    {
        public double W;
        public double X;
        public double Y;
        public double Z;

        public DifferentAssembly(double w, double x, double y, double z)
        {
            W = w;
            X = x;
            Y = y;
            Z = z;
        }

        // by-val
        public static DifferentAssembly operator +(DifferentAssembly a, DifferentAssembly b)
            => new DifferentAssembly(
                a.W + b.W,
                a.X + b.X,
                a.Y + b.Y,
                a.Z + b.Z);

        // by-ref-readonly
        public static DifferentAssembly operator *(in DifferentAssembly a, in DifferentAssembly b)
            => new DifferentAssembly(
                a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
                a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
                a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
                a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X);
    }
}
