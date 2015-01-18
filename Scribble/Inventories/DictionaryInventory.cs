using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;

namespace Inventories
{
    /// <summary>
    /// A simple implementation of <see cref="IInventory{T}"/> by using <see cref="Dictionary{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DictionaryInventory<T> : IObservableInventory<T>, IChangeable<T>, IEnumerable<T>
        where T : IIdentifiable
    {
        Dictionary<int, T> _items = new Dictionary<int, T>();

        public T GetItem(int id)
        {
            T x;
            return _items.TryGetValue(id, out x) ? x : default(T);
        }

        public IEnumerable<T> Items { get { return _items.Values; } }
        public IEnumerable<int> Ids { get { return _items.Keys; } }

        public IObservable<EventPattern<ItemChangedArgs<T>>> Changed { get { return _Changed; } }
        private Subject<EventPattern<ItemChangedArgs<T>>> _Changed = new Subject<EventPattern<ItemChangedArgs<T>>>();

        private IEnumerable<T> Except(IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first == null && second == null) return null;
            if (first == null) return second.ToArray();
            if (second == null) return first.ToArray();
            return first.Except(second, IdComparer<T>.Singleton).ToArray();
        }

        public void Change(ItemChangedArgs<T> args)
        {
            switch (args.Action)
            {
                case ItemChangedAction.Add:
                    _items.Add(args.Item.Id, args.Item);
                    break;
                case ItemChangedAction.Remove:
                    if (args.Id.HasValue)
                    {
                        var id = args.Id.Value;
                        T removed;
                        if (_items.TryGetValue(id, out removed))
                            args = ItemChangedArgs.Remove(removed);
                        _items.Remove(id);
                    }
                    else _items.Remove(args.Item.Id);
                    break;
                case ItemChangedAction.Update:
                    {
                        T removed;
                        if (_items.TryGetValue(args.Item.Id, out removed))
                            args = ItemChangedArgs.Update(args.Item, removed);
                        _items[args.Item.Id] = args.Item;
                    }
                    break;
                case ItemChangedAction.Reset:
                    {
                        var newItems = args.NewItems;
                        var oldItems = _items.Values;
                        var removed = Except(oldItems, newItems);
                        var added = Except(newItems, oldItems);

                        _items.Clear();
                        if (newItems != null)
                            foreach (var item in newItems)
                                _items[item.Id] = item;

                        args = ItemChangedArgs.Reset(newItems, oldItems);
                    }
                    break;
                default:
                    throw new ArgumentException("Invalid arg.Action");
            }
            _Changed.OnNext(this, args);
        }

        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
    }
}
