using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        //Test();

        //var x = new DictionaryBenchmark();
        //x.Setup();
        //x.Fixed();
        //x.Fixed2();

        BenchmarkRunner.Run<DictionaryBenchmark>();
    }

    private static void Test()
    {
        {
            var items = new[]
            {
                ("", 1),
                ("abc", 2),
                ("abcd", 3),
                ("abc1", 4),
                ("aaa", 5),
                ("aab", 6),
                ("aaa1", 7),
                ("aaa2", 8),
            };
            var otherKeys = new[] { "a", "abc2", "aa" };
            Test(items, otherKeys);
        }

        var allKeys = TestData.TypeNames.SelectMany(x => x.propertyNames).Distinct().ToArray();

        foreach (var (t, p) in TestData.TypeNames)
        {
            Console.WriteLine(t);
            Test(p.Select(x => (x, x)), allKeys.Except(p));
        }
    }

    private static void Test<T>(IEnumerable<(string, T)> items, IEnumerable<string> otherKeys)
    {
#if Char
        var n = new CharNode<T>();
#else
        var n = new LongNode<T>();
#endif

        foreach (var (key, value) in items)
        {
            n.Add(key, value);
        }

        foreach (var (key, value) in items)
        {
            var v = n[key];
            if (!EqualityComparer<T>.Default.Equals(value, v)) throw new Exception();
            if (!n.TryGetValue(key, out _)) throw new Exception();
        }

        foreach (var key in otherKeys)
        {
            if (n.TryGetValue(key, out _)) throw new Exception();
        }

        Console.WriteLine(n);
    }
}
