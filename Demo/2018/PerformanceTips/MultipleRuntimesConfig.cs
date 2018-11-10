using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Toolchains.CsProj;

public class MultipleRuntimesConfig : ManualConfig
{
    public MultipleRuntimesConfig()
    {
        Add(Job.Default.With(CsProjCoreToolchain.NetCoreApp20));
        Add(Job.Default.With(CsProjCoreToolchain.NetCoreApp21));

        Add(DefaultColumnProviders.Instance);
        Add(MarkdownExporter.GitHub);
        Add(new ConsoleLogger());
        Add(new HtmlExporter());
        Add(MemoryDiagnoser.Default);
    }
}
