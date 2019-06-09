namespace PatternBased.Queryable1
{
    using System;

    struct Queryable
    {
        public Queryable Where(Func<int, bool> f) => this;
        public Queryable Select(Func<int, int> f) => this;
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
