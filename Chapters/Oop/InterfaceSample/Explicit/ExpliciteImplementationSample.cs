namespace InterfaceSample.Explicit
{
    using System;
    using System.Collections.Generic;

    interface IAccumulator
    {
        void Add(int value);
        int Sum { get; }
    }

    interface IGroup<T>
    {
        void Add(T item);
        IEnumerable<T> Items { get; }
    }

    /// <summary>
    /// 1つの<see cref="Add(int)"/>で、2つのインターフェイスの実装を担うんであれば特に問題は出ない。
    /// </summary>
    class ImplicitImplementation : IAccumulator, IGroup<int>
    {
        public void Add(int x)
        {
            Sum += x;
            _items.Add(x);
        }

        public IEnumerable<int> Items => _items;
        private List<int> _items = new List<int>();

        public int Sum { get; private set; }
    }

    /// <summary>
    /// <see cref="IAccumulator.Add(int)"/>と、<see cref="IGroup{int}.Add(int)"/>が完全に被るので、
    /// 別の実装を与えたければ明示的実装が必要。
    /// </summary>
    class ExplicitImplementation : IAccumulator, IGroup<int>
    {
        void IAccumulator.Add(int value) => Sum += value;

        void IGroup<int>.Add(int item) => _items.Add(item);

        public IEnumerable<int> Items => _items;
        private List<int> _items = new List<int>();

        public int Sum { get; private set; }
    }

    class ExpliciteImplementationSample
    {
        public static void Main()
        {
            // 1つのAddで両方の債務を担ってるので2重集計される
            var a = new ImplicitImplementation();
            for (int i = 0; i < 5; i++)
            {
                Accumulate(a, i);
                AddItem(a, i);
            }
            Console.WriteLine($"sum = {a.Sum}, items = {string.Join(", ", a.Items)}");

            // 明示的実装を使って2つのAddを別実装したので個別集計される。
            var b = new ExplicitImplementation();
            for (int i = 0; i < 5; i++)
            {
                Accumulate(b, i);
                AddItem(b, i);
            }
            Console.WriteLine($"sum = {b.Sum}, items = {string.Join(", ", b.Items)}");
        }

        static void Accumulate(IAccumulator x, int value) => x.Add(value);

        static void AddItem<T>(IGroup<T> g, T item) => g.Add(item);
    }
}
