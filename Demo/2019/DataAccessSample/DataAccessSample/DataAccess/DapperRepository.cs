using Dapper;
using DataAccessSample.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessSample
{
    public struct DapperRepository : IDataSource
    {
        public Products[] GetAllProductsByCategory(NorthwindContext db, string categoryName)
        {
            var connection = db.Database.GetDbConnection();
            return connection.Query<Products>(@"SELECT * From Products as b
LEFT JOIN Categories as c ON c.CategoryID = b.CategoryID
WHERE c.CategoryName = @CategoryName
ORDER BY b.ProductName
", new { CategoryName = categoryName })
.ToArray();
        }

        public Task<Products[]> GetAllProductsByCategoryAsync(NorthwindContext db, string categoryName)
        {
            var connection = db.Database.GetDbConnection();
            return connection.QueryAsync<Products>(@"SELECT * From Products as b
LEFT JOIN Categories as c ON c.CategoryID = b.CategoryID
WHERE c.CategoryName = @CategoryName
ORDER BY b.ProductName
", new { CategoryName = categoryName })
.ContinueWith(t => t.Result.ToArray(), TaskContinuationOptions.ExecuteSynchronously);
        }
    }
}
