``` ini

BenchmarkDotNet=v0.10.7, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312793 Hz, Resolution=301.8601 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.2046.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.2046.0


```
 | Method |     Mean |     Error |    StdDev |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
 |------- |---------:|----------:|----------:|---------:|---------:|---------:|----------:|
 |      A | 2.857 ms | 0.0308 ms | 0.0273 ms | 496.0938 | 496.0938 | 496.0938 | 1875096 B |
 |      B | 2.568 ms | 0.0060 ms | 0.0047 ms | 164.0625 | 164.0625 | 164.0625 |  524312 B |
 |      C | 2.984 ms | 0.0104 ms | 0.0098 ms | 496.0938 | 496.0938 | 496.0938 | 1875096 B |
 |      D | 2.524 ms | 0.0110 ms | 0.0103 ms |        - |        - |        - |       0 B |
