using BenchmarkDotNet.Attributes;
using DataAccessSample.Models;

namespace DataAccessSample
{

    [MemoryDiagnoser]
    public class DataAccessBenchmark
    {
        private Products[][] GetProducts<T>()
            where T : struct, IDataSource
        {
            var shape = default(T);

            using (var db = new NorthwindContext())
            {
                return new[]
                {
                    shape.GetAllProductsByCategory(db, "Confections"),
                    shape.GetAllProductsByCategory(db, "Beverages"),
                    shape.GetAllProductsByCategory(db, "Produce"),
                    shape.GetAllProductsByCategory(db, "Seafood"),
                };
            }
        }

        [Benchmark] public Products[][] EFCore() => GetProducts<EFCoreRepository>();
        [Benchmark] public Products[][] EFCompiledQuery() => GetProducts<EFCompiledQueryRepository>();
        [Benchmark] public Products[][] EFFromSql() => GetProducts<EFFromSqlRepository>();
        [Benchmark] public Products[][] Dapper() => GetProducts<DapperRepository>();
    }
}
