using Dapper;
using DataAccessSample.Models;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DataAccessSample
{
    public struct DapperRepository : IDataSource
    {
        public Products[] GetAllProductsByCategory(string categoryName)
        {
            using (IDbConnection db = new SqlConnection(Program.ConnectionString))
            {
                return db.Query<Products>
                (@"SELECT * From [Products] as [b]
LEFT JOIN [Categories] as [c] ON [c].[CategoryID] = [b].[CategoryID]
WHERE [c].[CategoryName] = @CategoryName
ORDER BY [b].[ProductName]
", new { CategoryName = categoryName })
.ToArray();

            }
        }
    }
}
