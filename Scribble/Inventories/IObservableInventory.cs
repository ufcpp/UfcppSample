using System;
using System.Reactive;

namespace Inventories
{
    /// <summary>
    /// A repository which notifies an item-changed event.
    /// </summary>
    /// <typeparam name="T">Type of items</typeparam>
    public interface IObservableInventory<T> : IInventory<T>
        where T : IIdentifiable
    {
        /// <summary>
        /// An event raised on any item on the repository changed.
        /// </summary>
        IObservable<EventPattern<ItemChangedArgs<T>>> Changed { get; }
    }
}
