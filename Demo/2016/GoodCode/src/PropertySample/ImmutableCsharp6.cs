namespace PropertySample.ImmutableCsharp6
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
        public int X { get; }
        public Sample(int x) { X = x; }
    }
}
