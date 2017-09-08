using BenchmarkDotNet.Attributes;
using System.Text;

namespace Grisu3DoubleConversion
{
    public class DoubleConversionBenchmark
    {
        TestData data;

        [GlobalSetup]
        public void Setup()
        {
            data = new TestData(1000);
        }

        [Benchmark]
        public void SystemToString()
        {
            foreach (var x in data.DoubleValues) SystemToString(x);
            foreach (var x in data.SingleValues) SystemToString(x);
        }

        [Benchmark]
        public void SystemGetBytes()
        {
            foreach (var x in data.DoubleValues) SystemGetBytes(x);
            foreach (var x in data.SingleValues) SystemGetBytes(x);
        }

        [Benchmark]
        public void Grisu3ToString()
        {
            foreach (var x in data.DoubleValues) Grisu3ToString(x);
            foreach (var x in data.SingleValues) Grisu3ToString(x);
        }

        [Benchmark]
        public void Grisu3GetBytes()
        {
            foreach (var x in data.DoubleValues) Grisu3GetBytes(x);
            foreach (var x in data.SingleValues) Grisu3GetBytes(x);
        }

        public static string SystemToString(double x) => x.ToString();
        public static string SystemToString(float x) => x.ToString();

        public static string Grisu3ToString(double x)
        {
            DoubleConversion.ToString(x, out var buffer);
            return buffer.ToString();
        }

        public static string Grisu3ToString(float x)
        {
            DoubleConversion.ToString(x, out var buffer);
            return buffer.ToString();
        }

        public static byte[] SystemGetBytes(double x) => Encoding.UTF8.GetBytes(x.ToString());
        public static byte[] SystemGetBytes(float x) => Encoding.UTF8.GetBytes(x.ToString());

        public static byte[] Grisu3GetBytes(double x)
        {
            DoubleConversion.ToString(x, out var buffer);
            return buffer.GetUtf8Bytes();
        }

        public static byte[] Grisu3GetBytes(float x)
        {
            DoubleConversion.ToString(x, out var buffer);
            return buffer.GetUtf8Bytes();
        }
    }
}
