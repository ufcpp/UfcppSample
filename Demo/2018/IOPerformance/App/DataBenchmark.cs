using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace App
{
    public class DataBenchmark
    {
        [Benchmark]
        public IEnumerable<ResultData> Legacy()
        {
            LoadLegacy();
            return null;
            var r = Reduce(LoadLegacy());
            //Serialize(r, "legacyresult.json");
            return r;
        }

        [Benchmark]
        public IEnumerable<ResultData> SpanBased()
        {
            Load();
            return null;
            var r = Reduce(Load());
            //Serialize(r, "result.json");
            return r;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IEnumerable<TestData> LoadLegacy()
        {
            var list = new List<TestData>();
            using (var reader = new StreamReader("test.csv"))
            {
                reader.ReadLine();
                while (reader.ReadLine() is string s)
                {
                    var columns = s.Split(',');
                    list.Add(new TestData
                    {
                        a = columns[0],
                        b = columns[1],
                        x = double.Parse(columns[2]),
                        y = double.Parse(columns[3])
                    });
                }
            }
            return list;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IEnumerable<TestData> Load()
        {
            var list = new List<TestData>();
            using (var f = File.OpenRead("test.csv"))
            {
                Span<byte> initialBuffer = stackalloc byte[512];
                Span<int> indexBuffer = stackalloc int[4];
                ReadOnlySpan<byte> delimiter = stackalloc[] { (byte)',' };

                var reader = new LineReader(f, initialBuffer);

                var line = reader.ReadLine();
                while (true)
                {
                    line = reader.ReadLine();
                    if (line.IsEmpty) break;

                    var items = new Splitter(line, indexBuffer, delimiter);
                    list.Add(new TestData
                    {
                        a = GetString(items[0]),
                        b = GetString(items[1]),
                        x = GetDouble(items[2]),
                        y = GetDouble(items[3]),
                    });
                }
            }
            return list;

            string GetString(ReadOnlySpan<byte> s) => Encoding.UTF8.GetString(s);
            double GetDouble(ReadOnlySpan<byte> s) => Utf8Parser.TryParse(s, out double value, out _) ? value : 0;
        }

        private static IEnumerable<ResultData> Reduce(IEnumerable<TestData> testData)
            => testData
            .Select(d => (d.a, z: MultiplyToInt(d.x, d.y)))
            .GroupBy(d => d.a)
            .Select(g => new ResultData { a = g.Key, sum = g.Sum(x => x.z) });

        private static int MultiplyToInt(double x, double y)
        {
            if (x > 0)
                return (int)(x * y + 0.0000001);
            return (int)(x * y - 0.0000001);
        }

        private static void Serialize(IEnumerable<ResultData> data, string path)
        {
            using (var file = File.CreateText(path))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, data);
            }
        }
    }

    public struct ResultData
    {
        public string a;
        public int sum;
    }

    public struct TestData
    {
        public string a;
        public string b;
        public double x;
        public double y;
    }
}
