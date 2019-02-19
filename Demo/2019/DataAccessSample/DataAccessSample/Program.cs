using DataAccessSample.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DataAccessSample
{
    class Program
    {
        static void Main()
        {
#if DEBUG
            Test();
#else
            BenchmarkDotNet.Running.BenchmarkRunner.Run<DataAccessBenchmark>();
#endif
        }

        private static void Test()
        {
            using (var lease = Connection.Rent())
            {
                var db = lease.Context;
                var products = db.Products
                    .Where(b => b.CategoryId == 1)
                    .OrderBy(b => b.ProductId)
                    .Include(p => p.Category);
                Console.WriteLine(products.ToSql());

                WriteProducts<EFCoreRepository>(db);
                WriteProducts<EFCompiledQueryRepository>(db);
                WriteProducts<EFFromSqlRepository>(db);
                WriteProducts<DapperRepository>(db);
            }
        }

        private static void WriteProducts<T>(NorthwindContext db)
            where T : struct, IDataSource
        {
            Console.WriteLine(typeof(T).Name);

            foreach (var p in default(T).GetAllProductsByCategory(db, "Confections"))
            {
                Console.Write("    ");
                Console.WriteLine(p.ProductName);
            }

            foreach (var p in default(T).GetAllProductsByCategoryAsync(db, "Confections").Result)
            {
                Console.Write("    ");
                Console.WriteLine(p.ProductName);
            }
        }
    }
}
