using DataAccessSample.Models;

namespace DataAccessSample
{
    public interface IDataSource
    {
        Products[] GetAllProductsByCategory(string categoryName);
    }
}
