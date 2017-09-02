using BenchmarkDotNet.Running;
using System;
using System.Text;

public class Program
{
    static void Main()
    {
#if tru
        var x = new FindBenchmark();
        x.Setup();
        Console.WriteLine(x.OrdinalIndexOf());
        Console.WriteLine(x.BoyerMooreIndexOf());
#else
        //BenchmarkRunner.Run<StringComparisonBenchmark>();
        BenchmarkRunner.Run<Utf8SearchBenchmark>();
#endif
    }

    private static void Find(byte[] data)
    {
        var utf8 = Encoding.UTF8;
        var pattern = utf8.GetBytes("タプル");

        Span<byte> str = data;
        while (true)
        {
            var i = BoyerMoore.IndexOf(str, pattern);
            if (str.Length == i) break;
            Console.WriteLine($"{i}: {utf8.GetString(str.Slice(i, pattern.Length).ToArray())}");
            str = str.Slice(i + 1);
        }
    }
}
