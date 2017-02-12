using Deconstruction._Vector2D;

namespace Deconstruction._Vector3D
{
    struct Vector3D
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public Vector3D(double x, double y, double z) => (X, Y, Z) = (x, y, z);

        // 引数の数が違えば大丈夫
        public void Deconstruct(out double x, out double y, out double z) => (x, y, z) = (X, Y, Z);
        public void Deconstruct(out double first, out Vector2D rest) => (first, rest) = (X, new Vector2D(Y, Z));
    }

    class Program
    {
        static void Main()
        {
            var p = new Vector3D(1, 2, 3);

            // 分解可能
            var (first, rest) = p;
            var (x, y, z) = p;
        }
    }
}
