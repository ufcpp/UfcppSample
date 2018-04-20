using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Newtonsoft.Json;

namespace app
{
    public class DataBenchmark
    {
        private static IEnumerable<TestData> Load()
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

        private static void Serialize(IEnumerable<ResultData> data)
        {
            using (var file = File.CreateText("result.json"))
            {
            var serializer = new JsonSerializer();
                serializer.Serialize(file, data);
            }
        }

        [Benchmark]
        public void Run()
        {
            var testData = Load();

            var results = testData
                .Select(d => (d.a, z: MultiplyToInt(d.x, d.y)))
                .GroupBy(d => d.a)
                .Select(g => new ResultData { a = g.Key, sum = g.Sum(x => x.z) });

            Serialize(results);
        }

        private static int MultiplyToInt(double x, double y)
        {
            if (x > 0)
                return (int)(x * y + 0.0000001);
            return (int)(x * y - 0.0000001);
        }
    }

    internal struct ResultData
    {
        public string a;
        public int sum;
    }

    internal struct TestData
    {
        public string a;
        public string b;
        public double x;
        public double y;
        public int z;
    }
}
