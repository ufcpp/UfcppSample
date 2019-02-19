using DataAccessSample.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DataAccessSample
{
    class Program
    {
#if MYSQL
        public static readonly string ConnectionString = @"Server=localhost;Port=3306;Database=Northwind;Uid=root;Pwd=mypassword;Max Pool Size=100;";
#else
        public static readonly string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Max Pool Size=100";
#endif
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
            using (var db = new NorthwindContext())
            {
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
