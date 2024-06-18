using ValueList;

#if DEBUG

var b = new ValueListBenchmark();
b.Setup();
const int Take = 200;
foreach (var x in b.ValueList().Span[..Take]) Console.Write($"{x:X2}"); Console.WriteLine();
foreach (var x in b.PooledValueList().Span[..Take]) Console.Write($"{x:X2}"); Console.WriteLine();
foreach (var x in b.ValueListBuilder().Span[..Take]) Console.Write($"{x:X2}"); Console.WriteLine();
foreach (var x in b.ReferenceImplementation().Span[..Take]) Console.Write($"{x:X2}"); Console.WriteLine();

#else
BenchmarkDotNet.Running.BenchmarkRunner.Run<ValueListBenchmark>();

#endif
