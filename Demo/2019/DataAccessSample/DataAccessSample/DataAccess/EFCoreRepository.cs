using System.Linq;
using System.Threading.Tasks;
using DataAccessSample.Models;
using Microsoft.EntityFrameworkCore;

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

        public Task<Products[]> GetAllProductsByCategoryAsync(NorthwindContext db, string categoryName)
        {
            return db.Products
                .Where(b => b.Category.CategoryName == categoryName)
                .OrderBy(b => b.ProductName)
                .ToArrayAsync();
        }
    }
}
