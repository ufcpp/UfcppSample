using System.Linq;
using System.Threading.Tasks;
using DataAccessSample.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessSample
{
    public struct EFFromSqlRepository : IDataSource
    {
        public Products[] GetAllProductsByCategory(NorthwindContext db, string categoryName)
        {
            return db.Products.FromSql($@"SELECT b.ProductID, b.CategoryID, b.Discontinued, b.ProductName, b.QuantityPerUnit, b.ReorderLevel, b.SupplierID, b.UnitPrice, b.UnitsInStock, b.UnitsOnOrder
From Products as b
LEFT JOIN Categories as c ON c.CategoryID = b.CategoryID
WHERE c.CategoryName = {categoryName}
ORDER BY b.ProductName
").ToArray();
        }

        public Task<Products[]> GetAllProductsByCategoryAsync(NorthwindContext db, string categoryName)
        {
            return db.Products.FromSql($@"SELECT b.ProductID, b.CategoryID, b.Discontinued, b.ProductName, b.QuantityPerUnit, b.ReorderLevel, b.SupplierID, b.UnitPrice, b.UnitsInStock, b.UnitsOnOrder
From Products as b
LEFT JOIN Categories as c ON c.CategoryID = b.CategoryID
WHERE c.CategoryName = {categoryName}
ORDER BY b.ProductName
").ToArrayAsync();
        }
    }
}
