using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace Inventories
{
    /// <summary>
    /// A simple implementation of <see cref="IObservableEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class ObservableEnumerable<T> : IObservableEnumerable<T>
    {
        public IEnumerable<T> Items { get; }
        public IObservable<T> Updated { get; }

        public ObservableEnumerable(IEnumerable<T> items, IObservable<T> updated)
        {
            Items = items;
            Updated = updated;
        }
    }

    /// <summary>
    /// An implementation of <see cref="IObservableEnumerable{T}"/>.
    /// </summary>
    public static class ObservableEnumerable
    {
        /// <summary>
        /// Creates a new <see cref="IObservableEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Current items.</param>
        /// <param name="updated">Item-updated event.</param>
        /// <returns></returns>
        public static IObservableEnumerable<T> Create<T>(IEnumerable<T> items, IObservable<T> updated) => new ObservableEnumerable<T>(items, updated);

        /// <summary>
        /// Filters items and its updated event based on a predicate.
        /// </summary>
        /// <typeparam name="TSource">Type of items.</typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IObservableEnumerable<TSource> Where<TSource>(this IObservableEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            Create(source.Items.Where(predicate), source.Updated.Where(predicate));

        /// <summary>
        /// Projects items and its updated event into a new form.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IObservableEnumerable<TResult> Select<TSource, TResult>(this IObservableEnumerable<TSource> source, Func<TSource, TResult> selector) =>
            Create(source.Items.Select(selector), source.Updated.Select(selector));

        /// <summary>
        /// <paramref name="observer"/> is called immediately with a current first item, and then on updated.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="observer"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<T>(this IObservableEnumerable<T> source, Action<T> observer)
        {
            observer(source.Items.First());
            return source.Updated.Subscribe(observer);
        }
    }
}
