using System;

namespace LazyMixinDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var sample = new Sample();

            for (int i = 0; i < 4; i++)
            {
                sample.Counter.Add();
                Console.WriteLine(sample.Counter.Count);
            }

            sample.Reset();

            for (int i = 0; i < 4; i++)
            {
                sample.Counter.Add();
                Console.WriteLine(sample.Counter.Count);
            }
        }
    }
}
