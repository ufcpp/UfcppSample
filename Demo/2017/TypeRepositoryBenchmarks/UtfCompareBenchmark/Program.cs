using BenchmarkDotNet.Running;
using System;
using System.Linq;
using System.Text;

public class Program
{
    static void Main()
    {
#if tru
        var x = new StringBenchmark();
        x.Setup();
        x.EncodingGetBytesUnsafe();
        x.StringToUtf8Byte();
        x.EncodingGetBytes();
        x.EncodingGetString();
        x.EncodingGetCharsUnsafe();
        x.MyGetBytes();
#else
        BenchmarkRunner.Run<StringBenchmark>();
#endif
        //Test();
    }

    private static void Test()
    {
        var s = "🐭🐮🐯あいうαβγabc";
        var bytes = Encoding.UTF8.GetBytes(s);

        foreach (var x in s.AsBytes().Zip(bytes, (x, y) => (x, y)))
        {
            Console.WriteLine((x.x.ToString("X"), x.y.ToString("X")));
        }

        Console.WriteLine(Enumerable.SequenceEqual(s.AsBytes(), bytes));
    }
}