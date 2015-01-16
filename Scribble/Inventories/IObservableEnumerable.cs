using System;
using System.Collections.Generic;

namespace Inventories
{
    /// <summary>
    /// An items source which you can get current items and subscribe an item-updated event.
    /// </summary>
    /// <typeparam name="T">Type of items.</typeparam>
    public interface IObservableEnumerable<T>
    {
        /// <summary>
        /// Current items.
        /// </summary>
        IEnumerable<T> Items { get; }

        /// <summary>
        /// An event raised on items updated.
        /// </summary>
        IObservable<T> Updated { get; }
    }
}
