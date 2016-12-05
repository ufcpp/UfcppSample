using System;

namespace CardBattle.Lib
{
    public static class RandomExtensions
    {
        public static double NextDouble(this Random random, double minInclusive, double maxExclusive)
            => random.NextDouble() * (maxExclusive - minInclusive) + minInclusive;
    }
}
