using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Toolchains.DotNetCli;
using System.Diagnostics;

namespace StringManipulation.Benchmark
{
    class Program
    {
        static void Main()
        {
            // ReadOnlySpan NonPortableCast が「Generate Exception: Method not found」とか言って怒られて実行できない。
            // Span 絡みの API 変更入りまくってるから完全にバージョンが一致してないとたぶんこの手のエラーで死ぬ。
            // 2・3時間悩んで解決できなかったんで Span のシグネチャが安定するまでしばらく塩漬け…
            //BenchmarkRunner.Run<StringManipulationBenchmark>(new MyConfig());

            // 仕方なく Stopwatch 運用…
            const int N = 5000;
            var benchmark = new StringManipulationBenchmark();

            var sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < N; i++)
            {
                benchmark.Classic();
            }
            sw.Stop();
            System.Console.WriteLine(sw.Elapsed);
            sw.Reset();

            sw.Start();
            for (int i = 0; i < N; i++)
            {
                benchmark.Unsafe();
            }
            sw.Stop();
            System.Console.WriteLine(sw.Elapsed);
            sw.Reset();

            sw.Start();
            for (int i = 0; i < N; i++)
            {
                benchmark.SafeStackalloc();
            }
            sw.Stop();
            System.Console.WriteLine(sw.Elapsed);
            sw.Reset();

            sw.Start();
            for (int i = 0; i < N; i++)
            {
                benchmark.FullyTuned();
            }
            sw.Stop();
            System.Console.WriteLine(sw.Elapsed);
            sw.Reset();
        }
    }

    public class MyConfig : ManualConfig
    {
        public MyConfig()
        {
            Add(Job.Default.With(
                CsProjCoreToolchain.From(
                    new NetCoreAppSettings(
                        targetFrameworkMoniker: "netcoreapp2.1",
                        runtimeFrameworkVersion: "2.1.0-preview2-26130-06",
                        name: ".NET Core 2.1"))));

            Add(DefaultColumnProviders.Instance);
            Add(MarkdownExporter.GitHub);
            Add(MemoryDiagnoser.Default);
            Add(new ConsoleLogger());
            Add(new HtmlExporter());
        }
    }
}
