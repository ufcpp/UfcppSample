using Dapper;
using DataAccessSample.Models;
using System.Data;
using System.Linq;

namespace DataAccessSample
{
    public struct DapperRepository : IDataSource
    {
        public Products[] GetAllProductsByCategory(string categoryName)
        {
            using (IDbConnection db =
#if MYSQL
                new MySql.Data.MySqlClient.MySqlConnection(Program.ConnectionString)
#else
                new System.Data.SqlClient.SqlConnection(Program.ConnectionString)
#endif
                )
            {
                return db.Query<Products>(
#if MYSQL
@"SELECT * From `Northwind`.`Products` as `b`
LEFT JOIN `Northwind`.`Categories` as `c` ON `c`.`CategoryID` = `b`.`CategoryID`
WHERE `c`.`CategoryName` = @CategoryName
ORDER BY `b`.`ProductName`
"
#else
@"SELECT * From [Products] as [b]
LEFT JOIN [Categories] as [c] ON [c].[CategoryID] = [b].[CategoryID]
WHERE [c].[CategoryName] = @CategoryName
ORDER BY [b].[ProductName]
"
#endif
, new { CategoryName = categoryName })
.ToArray();

            }
        }
    }
}
