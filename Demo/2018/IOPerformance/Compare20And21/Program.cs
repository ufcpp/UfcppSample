using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;

namespace app
{
    class Program
    {
        static void Main()
        {
            //new DataBenchmark().Run();

            BenchmarkRunner.Run<DataBenchmark>(new MultipleRuntimesConfig());
        }
    }

    public class MultipleRuntimesConfig : ManualConfig
    {
        public MultipleRuntimesConfig()
        {
            Add(SetRun(Job.Default.With(CsProjCoreToolchain.NetCoreApp20)));
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
            job.Run.UnrollFactor = 1;
            job.Run.InvocationCount = 1;
            job.Run.WarmupCount = 1;
            job.Run.TargetCount = 1;
            job.Run.LaunchCount = 1;
            return job;
        }
    }
}
