namespace LazyMixinDemo
{
    class Counter
    {
        public int Count { get; private set; }

        public void Add() => Count++;
    }
}
