``` ini

BenchmarkDotNet=v0.10.9, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312787 Hz, Resolution=301.8606 ns, Timer=TSC
.NET Core SDK=2.0.0-preview2-006497
  [Host]     : .NET Core 2.0.0-preview2-25407-01 (Framework 4.6.00001.0), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.0-preview2-25407-01 (Framework 4.6.00001.0), 64bit RyuJIT


```
 |                   Method |       Mean |     Error |    StdDev |  Gen 0 | Allocated |
 |------------------------- |-----------:|----------:|----------:|-------:|----------:|
 |               ItemSwitch |   476.5 ns |  4.538 ns |  4.245 ns | 0.0639 |     272 B |
 |               ItemCustom |   916.9 ns |  3.646 ns |  3.232 ns | 0.0629 |     272 B |
 |           ItemDictionary | 1,092.8 ns | 28.030 ns | 33.368 ns | 0.0629 |     272 B |
 |              PointSwitch |   191.1 ns |  1.429 ns |  1.267 ns | 0.0434 |     184 B |
 |              PointCustom |   278.3 ns |  2.976 ns |  2.784 ns | 0.0434 |     184 B |
 |          PointDictionary |   349.1 ns |  3.487 ns |  3.262 ns | 0.0434 |     184 B |
 |  ItemImmutableDictionary | 1,804.7 ns | 19.790 ns | 18.511 ns | 0.0629 |     272 B |
 |     ItemSortedDictionary | 4,613.6 ns | 13.748 ns | 11.480 ns | 0.0610 |     272 B |
 |           ItemSortedList | 4,465.0 ns | 24.166 ns | 22.605 ns | 0.0610 |     272 B |
 | PointImmutableDictionary |   673.5 ns |  2.679 ns |  2.506 ns | 0.0429 |     184 B |
 |    PointSortedDictionary | 1,078.4 ns | 16.282 ns | 15.230 ns | 0.0420 |     184 B |
 |          PointSortedList | 1,069.7 ns | 10.616 ns |  9.931 ns | 0.0420 |     184 B |
