using System;

namespace SpanSample
{
    static class Ex
    {
        public static void Deconstruct<T>(this T[] array, out T head, out Span<T> rest) => new Span<T>(array).Deconstruct(out head, out rest);

        public static void Deconstruct<T>(this Span<T> span, out T head, out Span<T> rest)
        {
            head = span[0];
            rest = span.Slice(1);
        }
    }

    class Program
    {
        static void Main()
        {
            var array = new[] { 1, 2, 3, 4, 5 };

            var (x, (y, (z, rest))) = array;

            Console.WriteLine(x);
            Console.WriteLine(y);
            Console.WriteLine(z);
            Console.WriteLine("rest length: " + rest.Length);
        }
    }
}
