using System;

namespace Grisu3DoubleConversion
{
    class TestData
    {
        public double[] DoubleValues;
        public float[] SingleValues;

        public TestData(int n)
        {
            var r = new Random();

            var x = new double[3 * n + 1];
            x[0] = 2e15;
            var i = 1;
            for (; i <= n; i++) x[i] = r.Next() * Math.Pow(10, r.Next(1, 15));
            for (; i <= 2 * n; i++) x[i] = (2 * r.NextDouble() - 1) * Math.Pow(10, r.Next(-300, 300));
            for (; i <= 3 * n; i++) x[i] = (2 * r.NextDouble() - 1) * Math.Pow(10, r.Next(-5, 15));
            DoubleValues = x;

            var y = new float[3 * n + 1];
            y[0] = 2e7f;
            i = 1;
            for (; i <= n; i++) y[i] = (float)(r.Next() * Math.Pow(10, r.Next(1, 7)));
            for (; i <= 2 * n; i++) y[i] = (float)((2 * r.NextDouble() - 1) * Math.Pow(10, r.Next(-35, 35)));
            for (; i <= 3 * n; i++) y[i] = (float)((2 * r.NextDouble() - 1) * Math.Pow(10, r.Next(-5, 7)));
            SingleValues = y;
        }
    }
}
