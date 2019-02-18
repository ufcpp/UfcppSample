using System.Linq;
using DataAccessSample.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessSample
{
    public struct EFFromSqlRepository : IDataSource
    {
        public Products[] GetAllProductsByCategory(string categoryName)
        {
                #if MYSQL
#else
#endif

            using (var db = new NorthwindContext())
            {
#if MYSQL
                return db.Products.FromSql($@"SELECT `b`.`ProductID`, `b`.`CategoryID`, `b`.`Discontinued`, `b`.`ProductName`, `b`.`QuantityPerUnit`, `b`.`ReorderLevel`, `b`.`SupplierID`, `b`.`UnitPrice`, `b`.`UnitsInStock`, `b`.`UnitsOnOrder`
From `Northwind`.`Products` as `b`
LEFT JOIN `Northwind`.`Categories` as `c` ON `c`.`CategoryID` = `b`.`CategoryID`
WHERE `c`.`CategoryName` = {categoryName}
ORDER BY `b`.`ProductName`
").ToArray();
#else
                return db.Products.FromSql($@"SELECT [b].[ProductID], [b].[CategoryID], [b].[Discontinued], [b].[ProductName], [b].[QuantityPerUnit], [b].[ReorderLevel], [b].[SupplierID], [b].[UnitPrice], [b].[UnitsInStock], [b].[UnitsOnOrder]
From [Products] as [b]
LEFT JOIN [Categories] as [c] ON [c].[CategoryID] = [b].[CategoryID]
WHERE [c].[CategoryName] = {categoryName}
ORDER BY [b].[ProductName]
").ToArray();
#endif
            }
        }
    }
}
