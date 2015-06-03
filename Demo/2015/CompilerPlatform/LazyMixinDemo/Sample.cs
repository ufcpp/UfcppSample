using Laziness;

namespace LazyMixinDemo
{
    class Sample
    {
        public Counter Counter => _counter.Value;

        private LazyMixin<Counter> _counter;

        // readonly つけてしまうとどうなるか
        //public Counter Counter => _counter.Value;
        //private readonly LazyMixin<Counter> _counter;

        // プロパティだとどうなるか
        //public Counter Counter => _counter.Value;
        //private LazyMixin<Counter> _counter { get; }

        public void Reset() => _counter.Dispose();
    }
}
