namespace IteratorSample.UseSequence
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        public static void Run()
        {
            Write(new[] { 1, 2, 3, 4, 5 });
            Write(new List<int> { 1, 2, 3, 4, 5 });
        }

        private static void Write<T>(IEnumerable<T> data)
        {
            foreach (var x in data)
            {
                Console.WriteLine(x);
            }
        }
    }
}
