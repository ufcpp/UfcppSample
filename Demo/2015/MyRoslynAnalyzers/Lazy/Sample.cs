namespace Lazy
{
    class Sample
    {
        public X X => _x.Value;

        Laziness.LazyMixin<X> _x;
    }
}
