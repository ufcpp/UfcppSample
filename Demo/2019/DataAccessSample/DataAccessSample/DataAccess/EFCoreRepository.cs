using System.Linq;
using DataAccessSample.Models;

namespace DataAccessSample
{
    public struct EFCoreRepository : IDataSource
    {
        public Products[] GetAllProductsByCategory(string categoryName)
        {
            using (var db = new NorthwindContext())
            {
                return db.Products
                    .Where(b => b.Category.CategoryName == categoryName)
                    .OrderBy(b => b.ProductName)
                    .ToArray();
            }
        }
    }
}
