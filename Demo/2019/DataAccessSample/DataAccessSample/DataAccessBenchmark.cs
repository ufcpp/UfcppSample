using BenchmarkDotNet.Attributes;
using DataAccessSample.Models;

namespace DataAccessSample
{

    [MemoryDiagnoser]
    public class DataAccessBenchmark
    {
        private Products[][] GetProducts<T>()
            where T : struct, IDataSource
            => new[]
            {
                default(T).GetAllProductsByCategory("Confections"),
                default(T).GetAllProductsByCategory("Beverages"),
                default(T).GetAllProductsByCategory("Produce"),
                default(T).GetAllProductsByCategory("Seafood"),
            };

        [Benchmark] public Products[][] EFCore() => GetProducts<EFCoreRepository>();
        [Benchmark] public Products[][] EFCompiledQuery() => GetProducts<EFCompiledQueryRepository>();
        [Benchmark] public Products[][] EFFromSql() => GetProducts<EFFromSqlRepository>();
        [Benchmark] public Products[][] Dapper() => GetProducts<DapperRepository>();
    }
}
