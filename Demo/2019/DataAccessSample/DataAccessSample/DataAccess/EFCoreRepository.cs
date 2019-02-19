using System.Linq;
using DataAccessSample.Models;

namespace DataAccessSample
{
    public struct EFCoreRepository : IDataSource
    {
        public Products[] GetAllProductsByCategory(NorthwindContext db, string categoryName)
        {
            return db.Products
                .Where(b => b.Category.CategoryName == categoryName)
                .OrderBy(b => b.ProductName)
                .ToArray();
        }
    }
}
