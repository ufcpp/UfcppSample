3・4個のキーに対して ToLookup してて、それのアロケーションで GC がネックになったりすることが結構あり。
破壊的操作(source の Span の変更)を認めるならアロケーション0で似たことができるはずという作業。
多少遅いけども、アロケーションはきっちり0に。

| Method      | Mean     | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|------------ |---------:|---------:|---------:|-------:|-------:|----------:|
| Linq        | 817.5 ns | 15.56 ns | 14.55 ns | 0.2689 | 0.0010 |    2256 B |
| InPlaceSpan | 987.5 ns |  7.91 ns |  6.60 ns |      - |      - |         - |
