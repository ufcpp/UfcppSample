C# 10.0 でせっかく string interpolation が高速化されたのに、その中で `string.Join` とか使うと遅くなるよなぁ…
みたいな悩みがあったものの、`ISpanFormattable` ベースの `Join` メソッドを用意することで解消できそうという話。

コード:

* [`ISpanFormattable` ベースの `Join` メソッド](Joiner.cs)
* [ベンチマーク](JoinBenchmark.cs)

ベンチマーク内容は、↓みたいな2重配列を、

```cs
new[]
{
    new byte[] { 192, 168, 0, 1 },
    new byte[] { 255, 255, 0, 0 },
};
```

`192.168.0.1, 255.255.0.0` (10進)とか `C0.A8.0.1, FF.FF.0.0` (16進)とかに整形する処理を計測。

ベンチマーク対象:

* `StringJoin`: 普通に `string.Join` 利用
* `StringJoinX`: 普通に `string.Join` 利用で、`X` 書式指定
* `JoinerJoin`: `ISpanFormattable` ベースの `Join` 利用
* `JoinerJoinX`: `ISpanFormattable` ベースの `Join` 利用で、`X` 書式指定

ベンチマーク結果(一例):

|      Method |     Mean |   Error |  StdDev |  Gen 0 | Allocated |
|------------ |---------:|--------:|--------:|-------:|----------:|
|  StringJoin | 531.5 ns | 4.78 ns | 4.24 ns | 0.0982 |     824 B |
| StringJoinX | 984.7 ns | 8.70 ns | 8.14 ns | 0.1545 |   1,296 B |
|  JoinerJoin | 484.6 ns | 2.82 ns | 2.64 ns | 0.0467 |     392 B |
| JoinerJoinX | 640.6 ns | 1.28 ns | 1.07 ns | 0.0458 |     384 B |
