using BenchmarkDotNet.Attributes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccessSample
{
    [MemoryDiagnoser]
    public class DataAccessBenchmark
    {
        const int Parallelism = 50;
        const int LoopCount = 100;

        [GlobalSetup]
        public void Setup()
        {
            ThreadPool.SetMinThreads(Parallelism, Parallelism);
        }

        private void GetProducts<T>()
            where T : struct, IDataSource
        {
            Parallel.For(0, LoopCount, new ParallelOptions { MaxDegreeOfParallelism = Parallelism },
                _ =>
                {
                    using (var lease = Connection.Rent())
                    {
                        var db = lease.Context;
                        default(T).GetAllProductsByCategory(db, "Confections");
                        default(T).GetAllProductsByCategory(db, "Beverages");
                        default(T).GetAllProductsByCategory(db, "Produce");
                        default(T).GetAllProductsByCategory(db, "Seafood");
                    }
                });
        }

        [Benchmark] public void EFCore() => GetProducts<EFCoreRepository>();
        [Benchmark] public void EFCompiledQuery() => GetProducts<EFCompiledQueryRepository>();
        [Benchmark] public void EFFromSql() => GetProducts<EFFromSqlRepository>();
        [Benchmark] public void Dapper() => GetProducts<DapperRepository>();

        private async Task GetProductsAsync<T>()
        where T : struct, IDataSource
        {
            await Task.WhenAll(Enumerable.Range(0, LoopCount)
                .Select(async _ =>
                {
                    using (var lease = Connection.Rent())
                    {
                        var db = lease.Context;
                        return new[]
                        {
                            await default(T).GetAllProductsByCategoryAsync(db, "Confections"),
                            await default(T).GetAllProductsByCategoryAsync(db, "Beverages"),
                            await default(T).GetAllProductsByCategoryAsync(db, "Produce"),
                            await default(T).GetAllProductsByCategoryAsync(db, "Seafood"),
                        };
                    }
                }));
        }

        [Benchmark] public Task EFCoreAsync() => GetProductsAsync<EFCoreRepository>();
        [Benchmark] public Task EFCompiledQueryAsync() => GetProductsAsync<EFCompiledQueryRepository>();
        [Benchmark] public Task EFFromSqlAsync() => GetProductsAsync<EFFromSqlRepository>();
        [Benchmark] public Task DapperAsync() => GetProductsAsync<DapperRepository>();
    }
}
