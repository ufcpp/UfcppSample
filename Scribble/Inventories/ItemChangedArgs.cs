using System.Collections.Generic;

namespace Inventories
{
    /// <summary>
    /// arguments of an item-changed event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ItemChangedArgs<T>
    {
        /// <summary>
        /// Type of the event.
        /// </summary>
        public ItemChangedAction Action { get; }

        /// <summary>
        /// New item for the Add/Update action.
        /// Removed item for the Remove action.
        /// Null for the Reset action.
        /// </summary>
        public T Item { get; }

        /// <summary>
        /// An id for the Remove action. This is not used for the other actions.
        /// The Remove action doesn't need an item instance. It requires only id.
        /// If this has any value (not null), the Remove action ignores the Item property.
        /// </summary>
        public int? Id { get; }

        /// <summary>
        /// New (added) items for the Update action. This is not used for the other actions.
        /// </summary>
        public IEnumerable<T> NewItems { get; }

        /// <summary>
        /// Old (removed) items for the Update action. This is not used for the other actions.
        /// </summary>
        public IEnumerable<T> OldItems { get; }

        public ItemChangedArgs(ItemChangedAction action, T item) { Action = action; Item = item; }
        public ItemChangedArgs(ItemChangedAction action, int id) { Action = action; Id = id; }
        public ItemChangedArgs(ItemChangedAction action) { Action = action; }
        public ItemChangedArgs(ItemChangedAction action, IEnumerable<T> newItems, IEnumerable<T> oldItems) { Action = action; NewItems = newItems; OldItems = oldItems; }
        public ItemChangedArgs(ItemChangedAction action, T newItem, T oldItem) { Action = action; Item = newItem; OldItems = new[] { oldItem }; }
    }

    /// <summary>
    /// Factory for <see cref="ItemChangedArgs{T}"/>.
    /// </summary>
    public class ItemChangedArgs
    {
        public static ItemChangedArgs<T> Add<T>(T item) { return new ItemChangedArgs<T>(ItemChangedAction.Add, item); }
        public static ItemChangedArgs<T> Remove<T>(T item) { return new ItemChangedArgs<T>(ItemChangedAction.Remove, item); }
        public static ItemChangedArgs<T> Remove<T>(int id) { return new ItemChangedArgs<T>(ItemChangedAction.Remove, id); }
        public static ItemChangedArgs<T> Update<T>(T item) { return new ItemChangedArgs<T>(ItemChangedAction.Update, item); }
        public static ItemChangedArgs<T> Update<T>(T newItem, T oldItem) { return new ItemChangedArgs<T>(ItemChangedAction.Update, newItem, oldItem); }
        public static ItemChangedArgs<T> Reset<T>(IEnumerable<T> newItems, IEnumerable<T> oldItems) { return new ItemChangedArgs<T>(ItemChangedAction.Reset, newItems, oldItems); }
    }
}
