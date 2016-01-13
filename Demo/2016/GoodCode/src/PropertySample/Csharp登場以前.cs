namespace PropertySample.Csharp登場以前
{
    class Program
    {
        public void Run()
        {
            var s = new Sample();
            s.SetX(s.GetX() + 1);
        }
    }

    class Sample
    {
        private int _x;
        public int GetX() { return _x; }
        public void SetX(int x) { _x = x; }
    }
}
