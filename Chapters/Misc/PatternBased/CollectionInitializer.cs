namespace PatternBased.CollectionInitializer
{
    using System;
    using System.Collections;

    struct Adder : IEnumerable
    {
        public IEnumerator GetEnumerator() => throw new NotImplementedException();
    }

    static class AdderExtensions
    {
        public static void Add(this Adder a, int x, int dummy = 0) { }
    }

    class Program
    {
        static void Main()
        {

        }
    }
}
