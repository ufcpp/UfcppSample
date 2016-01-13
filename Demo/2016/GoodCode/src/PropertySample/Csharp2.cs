namespace PropertySample.Csharp2
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
        private int _x;
        public int X
        {
            get { return _x; }
            private set { _x = value; }
        }

        public Sample(int x)
        {
            _x = x;
        }
    }
}
