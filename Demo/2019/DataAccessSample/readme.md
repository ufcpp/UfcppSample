# EFCore のベンチマーク

データは [Northwind](https://github.com/Microsoft/sql-server-samples/tree/master/samples/databases/northwind-pubs)を使ってる。
[instnwnd.sql](https://github.com/Microsoft/sql-server-samples/tree/master/samples/databases/northwind-pubs)を実行した状態そのままでクエリ掛けてる。

EF は、DB First で、以下のコマンドで C# クラスを生成。

```PowerShell
Scaffold-DbContext "Data Source=[Database Name];Initial Catalog=Northwind;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
```

以下のクエリを実行。

```cs
db.Products
    .Where(b => b.Category.CategoryName == categoryName)
    .OrderBy(b => b.ProductName)
    .ToArray();
```

これと同じ結果を得るために、以下の4通りのコードを比較。

- Entity Framework Core を利用
  - EFCore: 普通に `IQueryable` の `Where`, `OrderBy` を使って O/R マップ
  - EFCompiledQuery: `EF.CompileQuery` を使う
  - EFFromSql: `FromSql(FromattableString)` を使う
- Dapper: Dapper を利用

Northwind に入ってるデータは、
Products が77件、Categories が8件。
ベンチマークでは `categoryName` に Confections, Beverages, Produce, Seafood の4つを渡して実行。

ローカルで動いてる SqlServer, MySQL に対して実行した場合のベンチマーク結果の一例は以下の通り。

SqlServer
|               Method |     Mean |    Error |   StdDev | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|--------------------- |---------:|---------:|---------:|------------:|------------:|------------:|--------------------:|
|               EFCore | 803.8 ms | 15.63 ms | 19.19 ms |   2000.0000 |   1000.0000 |           - |           271.37 KB |
|      EFCompiledQuery | 810.1 ms | 13.18 ms | 12.33 ms |   2000.0000 |   1000.0000 |           - |           217.59 KB |
|            EFFromSql | 841.0 ms | 16.33 ms | 16.77 ms |   2000.0000 |   1000.0000 |           - |           232.45 KB |
|               Dapper | 825.8 ms | 16.44 ms | 23.04 ms |   1000.0000 |           - |           - |           126.81 KB |
|          EFCoreAsync | 822.8 ms | 11.10 ms | 10.38 ms |   3000.0000 |   1000.0000 |           - |          3000.42 KB |
| EFCompiledQueryAsync | 853.6 ms | 25.88 ms | 39.52 ms |   3000.0000 |   1000.0000 |           - |          2242.38 KB |
|       EFFromSqlAsync | 842.3 ms | 16.54 ms | 19.69 ms |   3000.0000 |   1000.0000 |           - |          2717.88 KB |
|          DapperAsync | 844.2 ms | 16.67 ms | 36.59 ms |  11000.0000 |   2000.0000 |           - |          1658.16 KB |

MySQL
|               Method |      Mean |    Error |    StdDev |    Median | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|--------------------- |----------:|---------:|----------:|----------:|------------:|------------:|------------:|--------------------:|
|               EFCore |  84.10 ms | 2.200 ms |  5.985 ms |  82.19 ms |   4000.0000 |   1000.0000 |           - |           425.83 KB |
|      EFCompiledQuery |  79.58 ms | 1.582 ms |  3.789 ms |  79.24 ms |   4000.0000 |   1000.0000 |           - |           373.68 KB |
|            EFFromSql |  74.17 ms | 1.483 ms |  4.010 ms |  73.86 ms |   3000.0000 |   1000.0000 |           - |           366.09 KB |
|               Dapper | 135.87 ms | 2.717 ms |  5.364 ms | 135.44 ms |   2000.0000 |   1000.0000 |           - |           233.57 KB |
|          EFCoreAsync | 312.32 ms | 6.198 ms | 14.487 ms | 307.14 ms |   4000.0000 |   1000.0000 |           - |         22976.13 KB |
| EFCompiledQueryAsync | 278.02 ms | 5.557 ms | 14.640 ms | 274.81 ms |   3000.0000 |   1000.0000 |           - |         20132.05 KB |
|       EFFromSqlAsync | 287.32 ms | 7.173 ms |  7.675 ms | 287.14 ms |   3000.0000 |   1000.0000 |           - |         19776.27 KB |
|          DapperAsync | 343.05 ms | 6.811 ms | 15.787 ms | 342.32 ms |   2000.0000 |   1000.0000 |           - |         11550.02 KB |
