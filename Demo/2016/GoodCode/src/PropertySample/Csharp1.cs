namespace PropertySample.Csharp1
{
    class Program
    {
        public void Run()
        {
            var s = new Sample();
            s.X += 1;
        }
    }

    class Sample
    {
        private int _x;
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }
    }
}
