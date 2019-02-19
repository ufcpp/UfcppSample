using DataAccessSample.Models;

namespace DataAccessSample
{
    public interface IDataSource
    {
        Products[] GetAllProductsByCategory(NorthwindContext context, string categoryName);
    }
}
