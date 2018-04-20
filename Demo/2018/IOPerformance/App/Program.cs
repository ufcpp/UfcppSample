using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using System.Linq;
using System;
using System.Collections.Generic;

namespace App
{
    class Program
    {
        static void Main()
        {
            //var c = DataBenchmark.LoadLegacy();
            //var d = DataBenchmark.Load();

            //Console.WriteLine(c.Count());
            //Console.WriteLine(d.Count());

            //foreach (var (x, y) in c.Zip(d, (x, y) => (x, y)))
            //{
            //    if(x.x != y.x || x.y != y.y)
            //        Console.WriteLine((x.x - y.x, x.y - y.y));
            //}
            //Console.WriteLine(c.SequenceEqual(d, new TestComp()));

            //var bench = new DataBenchmark();
            //var a = bench.Legacy();
            //var b = bench.SpanBased();
            //Console.WriteLine(a.Count());
            //Console.WriteLine(b.Count());
            //Console.WriteLine(a.SequenceEqual(b, new ResultComp()));

            BenchmarkRunner.Run<DataBenchmark>(new MultipleRuntimesConfig());
        }

        class TestComp : IEqualityComparer<TestData>
        {
            public bool Equals(TestData x, TestData y) => x.a == y.a && x.b == y.b && Equals(x.x, y.x) && Equals(x.y, y.y);
            public int GetHashCode(TestData obj) => obj.GetHashCode();
            private bool Equals(double x, double y) => Math.Abs(x - y) < 1e-12;
        }

        class ResultComp : IEqualityComparer<ResultData>
        {
            public bool Equals(ResultData x, ResultData y) => x.a == y.a && x.sum == y.sum;
            public int GetHashCode(ResultData obj) => obj.GetHashCode();
        }
    }

    public class MultipleRuntimesConfig : ManualConfig
    {
        public MultipleRuntimesConfig()
        {
            Add(SetRun(Job.Default.With(CsProjCoreToolchain.NetCoreApp21)));

            Add(DefaultColumnProviders.Instance);
            Add(MarkdownExporter.GitHub);
            Add(new ConsoleLogger());
            Add(new HtmlExporter());
            Add(MemoryDiagnoser.Default);
        }

        private static Job SetRun(Job job)
        {
            // さすがに重いんで1回限り
            job.Run.UnrollFactor = 5;
            job.Run.InvocationCount = 5;
            job.Run.WarmupCount = 1;
            job.Run.TargetCount = 1;
            job.Run.LaunchCount = 1;
            return job;
        }
    }
}
