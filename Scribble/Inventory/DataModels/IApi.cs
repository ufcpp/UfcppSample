using Inventories;
using System.Threading.Tasks;

namespace InventoriesSample.DataModels
{
    /// <summary>
    /// An api interface to communicate with the server.
    /// </summary>
    public interface IApi
    {
        /// <summary>
        /// Logs in and receives all data.
        /// </summary>
        /// <param name="authToken">authentication token.</param>
        /// <returns>received game data.</returns>
        Task<SyncResult> Login(int authToken);

        /// <summary>
        /// Gets a new unit.
        /// </summary>
        /// <param name="masterId"></param>
        /// <returns>Difference in the unit inventory.</returns>
        Task<ItemChangedArgs<Unit>[]> HireUnit(int masterId);

        /// <summary>
        /// Fires an unit who is no longer needed.
        /// </summary>
        /// <param name="id">The unit instance id.</param>
        /// <returns>Difference in the unit inventory.</returns>
        Task<ItemChangedArgs<Unit>[]> FireUnit(int id);
    }
}
