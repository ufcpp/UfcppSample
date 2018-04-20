using BenchmarkDotNet.Attributes;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace App
{
    public class DataBenchmark
    {
        [Benchmark]
        public IEnumerable<TestData> LoadLegacy()
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

        [Benchmark]
        public IEnumerable<TestData> Load()
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
    }

    public struct TestData
    {
        public string a;
        public string b;
        public double x;
        public double y;
    }
}
