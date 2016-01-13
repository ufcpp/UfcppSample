#if RECORD

namespace PropertySample.ImmutableCsharp7
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

    class Sample(int X);
}

#endif
