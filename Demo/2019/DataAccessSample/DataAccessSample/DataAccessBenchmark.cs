using BenchmarkDotNet.Attributes;
using DataAccessSample.Models;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccessSample
{
    [MemoryDiagnoser]
    public class DataAccessBenchmark
    {
        private void GetProducts<T>()
            where T : struct, IDataSource
        {
            const int parallelism = 100;
            const int loopCount = 100;
            var shape = default(T);

            ThreadPool.SetMinThreads(parallelism, parallelism);

            Parallel.For(0, loopCount, new ParallelOptions { MaxDegreeOfParallelism = parallelism },
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
    }
}
