namespace InterfaceSample.Bcl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// 4次元上の点。
    /// <see cref="IReadOnlyList{T}"/> を実装している = <see cref="IEnumerable{T}"/>に加えて、インデックス指定で値を読める。
    /// </summary>
    class Point4D : IReadOnlyList<double>
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public double W { get; }

        public Point4D(double x, double y, double z, double w) { X = x; Y = y; Z = z; W = w; }

        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    default:
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    case 3: return W;
                }
            }
        }

        public int Count => 4;

        public IEnumerator<double> GetEnumerator()
        {
            yield return X;
            yield return Y;
            yield return Z;
            yield return W;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    class IReadOnlyListSample
    {
        public static void Main()
        {
            var p1 = new Point4D(1, 2, 3, 4);
            var p2 = new Point4D(3, 7, 5, 11);

            // X, Y, Z, W の代わりに 0, 1, 2, 3 のインデックスで値を読み出し
            var innerProduct = 0.0;
            for (int i = 0; i < 4; i++)
                innerProduct += p1[i] * p2[i];

            Console.WriteLine(innerProduct);
        }
    }
}
