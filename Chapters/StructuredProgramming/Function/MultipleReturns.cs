namespace Function.MultipleReturns
{
    class Program
    {
        struct SumCount
        {
            public int sum;
            public int count;
        }

        static SumCount Tally(int[] items)
        {
            var sum = 0;
            var count = 0;
            foreach (var x in items)
            {
                sum += x;
                count++;
            }
            return new SumCount { sum = sum, count = count };
        }
    }
}

namespace Function.MultipleReturnsByTuple
{
    class Program
    {
        static (int sum, int count) Tally(int[] items)
        {
            var sum = 0;
            var count = 0;
            foreach (var x in items)
            {
                sum += x;
                count++;
            }
            return (sum, count);
        }
    }
}
