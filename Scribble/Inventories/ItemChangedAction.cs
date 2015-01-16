namespace Inventories
{
    /// <summary>
    /// Type of item-changed event.
    /// </summary>
    public enum ItemChangedAction
    {
        /// <summary>
        /// Adds an item. This may throw an exception if a repository has an item of the same id.
        /// </summary>
        Add,

        /// <summary>
        /// Removes an item.
        /// </summary>
        Remove,

        /// <summary>
        /// Updates an item. Replaces an item if it has the same Id as a new one.
        /// </summary>
        Update,

        /// <summary>
        /// Removes all items, and then adds new items.
        /// </summary>
        Reset,
    }
}
