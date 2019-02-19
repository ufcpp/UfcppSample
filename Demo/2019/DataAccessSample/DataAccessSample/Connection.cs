using DataAccessSample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace DataAccessSample
{
    class Connection
    {
#if MYSQL
        private const string ConnectionString = @"Server=localhost;Port=3306;Database=Northwind;Uid=root;Pwd=mypassword;Max Pool Size=200;";
        private static readonly DbContextOptions<NorthwindContext> Options = new DbContextOptionsBuilder<NorthwindContext>().UseMySQL(ConnectionString).Options;
#else
        private const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Max Pool Size=200";
        private static readonly DbContextOptions<NorthwindContext> Options = new DbContextOptionsBuilder<NorthwindContext>().UseSqlServer(ConnectionString).Options;
#endif

        private static readonly DbContextPool<NorthwindContext> _pool = new DbContextPool<NorthwindContext>(Options);

        public static DbContextPool<NorthwindContext>.Lease Rent() => new DbContextPool<NorthwindContext>.Lease(_pool);
    }
}
