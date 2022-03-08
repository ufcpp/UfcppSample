// See https://aka.ms/new-console-template for more information
using StringJoin;

#if DEBUG
var b = new JoinBenchmark();

Console.WriteLine(b.StringJoin());
Console.WriteLine(b.JoinerJoin());
Console.WriteLine(b.StringJoinX());
Console.WriteLine(b.JoinerJoinX());
#else
BenchmarkDotNet.Running.BenchmarkRunner.Run<JoinBenchmark>();
#endif
