namespace InterfaceSample.Bcl
{
    using System;
    using System.Linq;

    /// <summary>
    /// 2次元上の点。
    /// <see cref="IComparable{T}"/> を実装している = 順序をつけられる。
    /// </summary>
    class Point2D : IComparable<Point2D>
    {
        public double X { get; }
        public double Y { get; }

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double Radius => Math.Sqrt(X * X + Y * Y);
        public double Angle => Math.Atan2(Y, X);

        /// <summary>
        /// 距離で順序を決める。
        /// 距離が全く同じなら偏角で順序付け。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Point2D other)
        {
            var r = Radius.CompareTo(other.Radius);
            if (r != 0) return r;
            return Angle.CompareTo(other.Angle);
        }
    }


    class IComparableSample
    {
        public static void Main()
        {
            const int N = 5;
            var rand = new Random();
            var data = Enumerable.Range(0, N).Select(_ => new Point2D(rand.NextDouble(), rand.NextDouble())).ToArray();

            Console.WriteLine("元:");
            foreach (var p in data) WriteLine(p);

            // 並べ替えの順序に使える
            Console.WriteLine("整列済み:");
            foreach (var p in data.OrderBy(x => x)) WriteLine(p);
        }

        private static void WriteLine(Point2D p)
        {
            Console.WriteLine($"({p.X:N3}, {p.Y:N3}), radius = {p.Radius:N3}, angle = {p.Angle:N3}");
        }
    }
}
