namespace LocalFunctions.ToArray2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static class MyEnumerable
    {
        public static U[] SelectToArray<T, U>(this T[] array, Func<T, U> selector)
        {
            IEnumerable<U> inner()
            {
                foreach (var x in array)
                    yield return selector(x);
            }

            return inner().ToArray();
        }
    }

    class Program
    {
        static void Main()
        {
            var input = new[] { 1, 2, 3, 4, 5, 6, 7 };
            var output = input.SelectToArray(x => x * x);
        }
    }
}
