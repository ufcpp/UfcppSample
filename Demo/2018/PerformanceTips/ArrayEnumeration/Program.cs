using BenchmarkDotNet.Running;

namespace ArrayEnumeration
{
    class Program
    {
        static void Main()
        {
            //var x = new ArrayEnumerationBenchmark();
            //x.Setup();
            //System.Console.WriteLine(x.InterfaceEnumeration());
            //System.Console.WriteLine(x.StructEnumeration());
            //System.Console.WriteLine(x.SpanEnumeration());

            BenchmarkRunner.Run<ArrayEnumerationBenchmark>();
        }
    }
}
