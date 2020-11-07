using System;
using System.Collections.Generic;
using System.Linq;

namespace MetricSpace
{
    public partial class NearestNeighbourList<TItem, TDistance>
        where TDistance : IComparable<TDistance>
    {
        private const int DefaultCapacity = 32;

        public interface INearestNeighbourList
        {
            bool Add(TItem item, TDistance distance);
            TDistance FurtherestDistance { get; }
            bool IsFull { get; }
        }

        public class UnlimitedList : INearestNeighbourList
        {
            private readonly List<(TItem, TDistance)> _items;

            public UnlimitedList() : this(DefaultCapacity) { }
            public UnlimitedList(int capacity) => _items = new List<(TItem, TDistance)>(capacity);

            public TDistance FurtherestDistance => default!;

            public bool IsFull => false;

            public bool Add(TItem item, TDistance distance)
            {
                _items.Add((item, distance));
                return true;
            }

            public TItem[] GetSortedArray() => _items.OrderBy(x => x.Item2).Select(x => x.Item1).ToArray();

            public void Clear() => _items.Clear();
        }

        public class List : INearestNeighbourList
        {
            public List(int maxCount, int capacity)
            {
                MaxCount = maxCount;
                queue = new PriorityQueue<TItem, TDistance>(capacity);
            }

            public List(int maxCount) : this(maxCount, DefaultCapacity) { }
            public List() : this(int.MaxValue, DefaultCapacity) { }

            private readonly PriorityQueue<TItem, TDistance> queue;
            public int MaxCount { get; }

            public int Count { get { return queue.Count; } }

            public bool Add(TItem item, TDistance distance)
            {
                if (queue.Count >= MaxCount)
                {
                    // If the distance of this item is less than the distance of the last item
                    // in our neighbour list then pop that neighbour off and push this one on
                    // otherwise don't even bother adding this item
                    if (distance.CompareTo(queue.GetHighestPriority()) < 0)
                    {
                        queue.Dequeue();
                        queue.Enqueue(item, distance);
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    queue.Enqueue(item, distance);
                    return true;
                }
            }

            public TDistance FurtherestDistance => queue.GetHighestPriority();
            public bool IsFull => Count == MaxCount;

            public TItem RemoveFurtherest()
            {
                return queue.Dequeue();
            }

            public TItem[] GetSortedArray()
            {
                var count = Count;
                var neighbourArray = new TItem[count];

                for (var index = 0; index < count; index++)
                {
                    var n = RemoveFurtherest();
                    neighbourArray[count - index - 1] = n;
                }

                return neighbourArray;
            }
        }
    }
}
