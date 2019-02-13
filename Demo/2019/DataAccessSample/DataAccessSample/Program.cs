using BenchmarkDotNet.Running;
using DataAccessSample.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DataAccessSample
{
    class Program
    {
        public static readonly string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        static void Main()
        {
#if DEBUG
            Test();
#else
            BenchmarkRunner.Run<DataAccessBenchmark>();
#endif
        }

        private static void Test()
        {
            using (var db = new NorthwindContext())
            {
                var products = db.Products
                    .Where(b => b.CategoryId == 1)
                    .OrderBy(b => b.ProductId)
                    .Include(p => p.Category);
                var xx = products.ToSql();
            }

            WriteProducts<EFCoreRepository>();
            WriteProducts<EFCompiledQueryRepository>();
            WriteProducts<EFFromSqlRepository>();
            WriteProducts<DapperRepository>();
        }

        private static void WriteProducts<T>()
            where T : struct, IDataSource
        {
            Console.WriteLine(typeof(T).Name);

            foreach (var p in default(T).GetAllProductsByCategory("Confections"))
            {
                Console.Write("    ");
                Console.WriteLine(p.ProductName);
            }
        }
    }
}
