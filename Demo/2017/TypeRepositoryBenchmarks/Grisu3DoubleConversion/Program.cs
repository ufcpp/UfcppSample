using BenchmarkDotNet.Running;

namespace Grisu3DoubleConversion
{
    class Program
    {
        static void Main(string[] args)
        {
            //DoubleConversionTest.Test();

            //var x = new DoubleConversionBenchmark();
            //x.Setup();
            //x.SystemToString();
            //x.SystemGetBytes();
            //x.Grisu3ToString();
            //x.Grisu3GetBytes();

            BenchmarkRunner.Run<DoubleConversionBenchmark>();
        }
    }
}
