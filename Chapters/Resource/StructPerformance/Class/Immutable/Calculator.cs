using System;
using System.Linq;

namespace Class.Immutable
{
    class Calculator : ICalculator<Vector>
    {
        public string Name => "Immutable Class ";

        public Vector[] GetSeries(Random r, int count) => Enumerable.Range(0, count).Select(_ => GetRandom(r)).ToArray();
        private static Vector GetRandom(Random r) => Get(() => r.NextDouble(-1, 1));
        private static Vector Get(Func<double> f) => new Vector(f(), f(), f(), f(), f(), f(), f(), f());

        public Vector SeriesSum(Vector[] seq)
        {
            var sum = new Vector();
            for (int i = 0; i < seq.Length; i++)
                sum = sum.Add(seq[i]);
            return sum;
        }
    }
}
