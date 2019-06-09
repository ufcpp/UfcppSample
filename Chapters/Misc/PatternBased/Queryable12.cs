namespace PatternBased.Queryable2
{
    using System;

    struct Queryable
    {
        public Queryable Where(Func<int, bool> f, params int[] dummy) => this;
    }

    static class QueryableExtensions
    {
        public static Queryable Select(this Queryable q, Func<int, int> f, int dummy = 0) => q;
    }

    class Program
    {
        static void Main()
        {
            var q =
                from x in new Queryable()
                where x < 10
                select x * x;
        }
    }
}
