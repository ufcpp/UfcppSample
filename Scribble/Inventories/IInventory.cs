using System.Collections.Generic;

namespace Inventories
{
    /// <summary>
    /// An item repository.
    /// Items in the repository must have an id.
    /// </summary>
    /// <typeparam name="T">Type of items</typeparam>
    public interface IInventory<T>
        where T : IIdentifiable
    {
        /// <summary>
        /// Gets all items in the repository.
        /// </summary>
        IEnumerable<T> Items { get; }

        /// <summary>
        /// Gets Ids of all items in the repository.
        /// </summary>
        IEnumerable<int> Ids { get; }

        /// <summary>
        /// Finds an item by id.
        /// </summary>
        /// <returns>The found item. Returns null if not found.</returns>
        T GetItem(int id);
    }
}
