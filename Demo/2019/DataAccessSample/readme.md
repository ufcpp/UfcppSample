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

ローカルで動いてる SqlServer に対して実行した場合のベンチマーク結果の一例は以下の通り。

|           Method |     Mean |     Error |    StdDev |   Median | Allocated Memory/Op |
| ---------------- |---------:|----------:|----------:|---------:|--------------------:|
|           EFCore | 11.51 ms | 0.2279 ms | 0.5096 ms | 11.44 ms |           237.13 KB |
|  EFCompiledQuery | 12.02 ms | 0.4997 ms | 1.4576 ms | 11.48 ms |           204.95 KB |
|        EFFromSql | 12.16 ms | 0.3847 ms | 1.0977 ms | 11.97 ms |           214.51 KB |
|           Dapper | 10.11 ms | 0.1492 ms | 0.1395 ms | 10.08 ms |            62.31 KB |