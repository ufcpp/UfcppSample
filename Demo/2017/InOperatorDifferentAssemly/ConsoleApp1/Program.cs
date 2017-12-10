using ClassLibrary1;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Legal();
            Error();
        }

        private static void Legal()
        {
            var a = new SameAssembly(1, 2, 3, 4);
            var b = new SameAssembly(1, -2, 3, -4);

            // No error.
            Console.WriteLine(a + b);

            // No error.
            Console.WriteLine(a * b);
        }

        private static void Error()
        {
            var a = new DifferentAssembly(1, 2, 3, 4);
            var b = new DifferentAssembly(1, -2, 3, -4);

            // pass by value. No error.
            Console.WriteLine(a + b);

            // pass by ref readonly.
            // No error in the Visual Studio C# Editor
            // but error on build.
            Console.WriteLine(a * b);
        }
    }

    /// <summary>
    /// The same as <see cref="ClassLibrary1.DifferentAssembly"/>.
    /// </summary>
    public struct SameAssembly
    {
        public double W;
        public double X;
        public double Y;
        public double Z;

        public SameAssembly(double w, double x, double y, double z)
        {
            W = w;
            X = x;
            Y = y;
            Z = z;
        }

        // by-val
        public static SameAssembly operator +(SameAssembly a, SameAssembly b)
            => new SameAssembly(
                a.W + b.W,
                a.X + b.X,
                a.Y + b.Y,
                a.Z + b.Z);

        // by-ref-readonly
        public static SameAssembly operator *(in SameAssembly a, in SameAssembly b)
            => new SameAssembly(
                a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
                a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
                a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
                a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X);
    }
}
