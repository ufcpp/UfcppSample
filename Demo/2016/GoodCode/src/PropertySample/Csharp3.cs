namespace PropertySample.Csharp3
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
        public int X { get; private set; }
        public Sample(int x) { X = x; }
    }
}
