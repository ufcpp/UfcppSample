using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;

namespace Inventories
{
    /// <summary>
    /// Extension methods for an event-pttern observable/observer, and repository.
    /// </summary>
    public static class Events
    {
        /// <summary>
        /// Subscribes an event without sender.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="observer"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<T>(this IObservable<EventPattern<ItemChangedArgs<T>>> e, Action<ItemChangedArgs<T>> observer) =>
            e.Subscribe(ep => observer(ep.EventArgs));

        /// <summary>
        /// A shorthand for event-pattern <see cref="IObserver{T}.OnNext(T)"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observer"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void OnNext<T>(this IObserver<EventPattern<T>> observer, object sender, T args) =>
            observer.OnNext(new EventPattern<T>(sender, args));

        /// <summary>
        /// Creates a new event-pattern observable which raises on an item updated in the repository.
        /// The observable yields:
        ///  - the Item on the Add/Update action,
        ///  - the NewItems the a Reset action,
        ///  - none on the Remove action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="r"></param>
        /// <returns></returns>
        public static IObservable<T> ItemUpdated<T>(this IObservableInventory<T> r) where T : IIdentifiable =>
            Observable.Create<T>(observer =>
                r.Changed.Subscribe(ep =>
                {
                    var action = ep.EventArgs.Action;

                    if (action == ItemChangedAction.Add || action == ItemChangedAction.Update)
                    {
                        observer.OnNext(ep.EventArgs.Item);
                    }
                    else if (action == ItemChangedAction.Reset && ep.EventArgs.NewItems != null)
                    {
                        foreach (var item in ep.EventArgs.NewItems)
                            observer.OnNext(item);
                    }
                }));

        public static void Add<T>(this IChangeable<T> r, T item) => r.Change(ItemChangedArgs.Add(item));
        public static void Remove<T>(this IChangeable<T> r, T item) => r.Change(ItemChangedArgs.Remove(item));
        public static void Remove<T>(this IChangeable<T> r, int id) => r.Change(ItemChangedArgs.Remove<T>(id));
        public static void Update<T>(this IChangeable<T> r, T item) => r.Change(ItemChangedArgs.Update(item));
        public static void Reset<T>(this IChangeable<T> r, IEnumerable<T> newItems) => r.Change(ItemChangedArgs.Reset(newItems, null));

        /// <summary>
        /// Projects <see cref="IObservableInventory{T}"/> to <see cref="IObservableEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="r"></param>
        /// <returns></returns>
        public static IObservableEnumerable<T> AsObservableEnumerable<T>(this IObservableInventory<T> r) where T : IIdentifiable =>
            ObservableEnumerable.Create(r.Items, r.ItemUpdated());
    }
}
