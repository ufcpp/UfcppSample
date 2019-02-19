using BenchmarkDotNet.Attributes;
using DataAccessSample.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccessSample
{
    [MemoryDiagnoser]
    public class DataAccessBenchmark
    {
        const int Parallelism = 100;
        const int LoopCount = 100;

        [GlobalSetup]
        public void Setup()
        {
            ThreadPool.SetMinThreads(Parallelism, Parallelism);
        }

        private void GetProducts<T>()
            where T : struct, IDataSource
        {
            var shape = default(T);

            Parallel.For(0, LoopCount, new ParallelOptions { MaxDegreeOfParallelism = Parallelism },
                () => new NorthwindContext(),
                (_, state, db) =>
                {
                    shape.GetAllProductsByCategory(db, "Confections");
                    shape.GetAllProductsByCategory(db, "Beverages");
                    shape.GetAllProductsByCategory(db, "Produce");
                    shape.GetAllProductsByCategory(db, "Seafood");

                    return db;
                },
                db => db.Dispose());
        }

        [Benchmark] public void EFCore() => GetProducts<EFCoreRepository>();
        [Benchmark] public void EFCompiledQuery() => GetProducts<EFCompiledQueryRepository>();
        [Benchmark] public void EFFromSql() => GetProducts<EFFromSqlRepository>();
        [Benchmark] public void Dapper() => GetProducts<DapperRepository>();

        private async Task GetProductsAsync<T>()
        where T : struct, IDataSource
        {
            var shape = default(T);

            using (var db = new NorthwindContext())
            {
                await Task.WhenAll(Enumerable.Range(0, LoopCount)
                    .SelectMany(_ => new[]
                    {
                        shape.GetAllProductsByCategoryAsync(db, "Confections"),
                        shape.GetAllProductsByCategoryAsync(db, "Beverages"),
                        shape.GetAllProductsByCategoryAsync(db, "Produce"),
                        shape.GetAllProductsByCategoryAsync(db, "Seafood"),
                    }));
            }
        }

        [Benchmark] public Task EFCoreAsync() => GetProductsAsync<EFCoreRepository>();
        [Benchmark] public Task EFCompiledQueryAsync() => GetProductsAsync<EFCompiledQueryRepository>();
        [Benchmark] public Task EFFromSqlAsync() => GetProductsAsync<EFFromSqlRepository>();
        [Benchmark] public Task DapperAsync() => GetProductsAsync<DapperRepository>();
    }
}
