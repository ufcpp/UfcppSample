using System;
using System.Collections.Generic;

namespace Deconstruction
{
    static class Extensions
    {
        public static void Deconstruct<T, U>(this KeyValuePair<T, U> pair, out T key, out U value)
        {
            key = pair.Key;
            value = pair.Value;
        }

        public static void Deconstruct<T1, T2>(this Tuple<T1, T2> x, out T1 item1, out T2 item2)
        {
            item1 = x.Item1;
            item2 = x.Item2;
        }
    }
}
