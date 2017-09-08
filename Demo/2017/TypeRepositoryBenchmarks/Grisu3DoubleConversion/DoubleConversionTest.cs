//#define VERBOSE

using System;
using System.Linq;

namespace Grisu3DoubleConversion
{
    class DoubleConversionTest
    {
        public static void Test()
        {
#if VERBOSE
            const int N = 100;
#else
            const int N = 100_000;
#endif
            var data = new TestData(N);

            foreach (var x in data.DoubleValues) Test(x);
            foreach (var x in data.SingleValues) Test(x);
        }

        private static void WriteLine((string, string) x)
        {
#if VERBOSE
            Console.WriteLine(x);
#endif
        }

        private static void Test(double x)
        {
            DoubleConversion.ToString(x, out var buffer);
            WriteLine((x.ToString(), buffer.ToString()));

            var xs = x.ToString();
            var ys = buffer.ToString();

            if (xs.Contains('E') != ys.Contains('E')) throw new Exception();

            var y = double.Parse(ys);
            var diff = Math.Abs((x - y) / x);
            if (diff > 1E-15) throw new Exception();
        }

        private static void Test(float x)
        {
            DoubleConversion.ToString(x, out var buffer);
            WriteLine((x.ToString(), buffer.ToString()));

            var xs = x.ToString();
            var ys = buffer.ToString();

            if(xs.Contains('E') != ys.Contains('E')) throw new Exception();

            var y = float.Parse(ys);
            var diff = Math.Abs((x - y) / x);
            if (diff > 2E-7) throw new Exception();
        }
    }
}
