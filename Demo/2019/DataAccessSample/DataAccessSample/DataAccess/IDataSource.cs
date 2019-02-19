using DataAccessSample.Models;
using System.Threading.Tasks;

namespace DataAccessSample
{
    public interface IDataSource
    {
        Products[] GetAllProductsByCategory(NorthwindContext db, string categoryName);
        Task<Products[]> GetAllProductsByCategoryAsync(NorthwindContext db, string categoryName);
    }
}
