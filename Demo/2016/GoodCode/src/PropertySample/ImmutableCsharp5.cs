namespace PropertySample.ImmutableCsharp5
{
    using static System.Console;

    class Program
    {
        public void Run()
        {
            var s = new Sample(1);
            WriteLine(s.X);
        }
    }

    class Sample
    {
        private readonly int _x;
        public int X { get { return _x; } }
        public Sample(int x) { _x = x; }
    }
}
