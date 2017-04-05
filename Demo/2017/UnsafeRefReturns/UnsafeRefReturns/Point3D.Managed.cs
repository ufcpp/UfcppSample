namespace UnsafeRefReturns
{
    namespace Managed
    {
        /// <summary>
        /// 配列の薄いラッパー。
        /// これくらいなら、set/get 用意したプロパティにしてもいいと言えばいいんだけども。
        /// たまに、<![CDATA[ ref p.X ]]> したいときに面倒。
        /// </summary>
        struct Point3D
        {
            int[] _data;
            private Point3D(params int[] data) => _data = data;
            public static Point3D New(int x, int y, int z) => new Point3D(x, y, z);
            public ref int X => ref _data[0];
            public ref int Y => ref _data[1];
            public ref int Z => ref _data[2];
        }

        struct Triangle
        {
            public Point3D[] _vertices;
            private Triangle(params Point3D[] vertices) => _vertices = vertices;
            public static Triangle New(Point3D a, Point3D b, Point3D c) => new Triangle(a, b, c);
            public ref Point3D A => ref _vertices[0];
            public ref Point3D B => ref _vertices[0];
            public ref Point3D C => ref _vertices[0];
        }

        class Program
        {
            static void Main()
            {
                var t = Triangle.New(Point3D.New(1, 0, 0), Point3D.New(0, 1, 0), Point3D.New(0, 0, 1));
                t.A.X = 10;
            }
        }
    }
}
