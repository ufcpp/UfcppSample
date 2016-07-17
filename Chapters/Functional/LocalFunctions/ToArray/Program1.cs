namespace LocalFunctions.ToArray1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static class MyEnumerable
    {
        public static U[] SelectToArray<T, U>(this T[] array, Func<T, U> selector)
        {
            return Select(array, selector).ToArray();
        }

        // SelectToArray からしか呼ばれない
        private static IEnumerable<U> Select<T, U>(IEnumerable<T> source, Func<T, U> selector)
        {
            foreach (var x in source)
                yield return selector(x);
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
