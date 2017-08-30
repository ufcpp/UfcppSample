using BenchmarkDotNet.Running;
using System;
using System.Text;

class Program
{
    static void Main()
    {
        Test();

        BenchmarkRunner.Run<ParseIntBenchmark>();
    }

    private static void Test()
    {
        var r = new Random();

        for (int i = 0; i < 128; i++)
        {
            var x = r.Next();
            var str = Encoding.ASCII.GetBytes(x.ToString());

            var a = ParseIntBenchmark.ParseA(str);
            var b = ParseIntBenchmark.ParseB(str);
            var c = ParseIntBenchmark.ParseC(str);
            var d = ParseIntBenchmark.ParseC(str);
            var e = ParseIntBenchmark.ParseE(str);

            if (x != a) throw new Exception();
            if (x != b) throw new Exception();
            if (x != c) throw new Exception();
            if (x != d) throw new Exception();
            if (x != e) throw new Exception();
        }
    }
}
