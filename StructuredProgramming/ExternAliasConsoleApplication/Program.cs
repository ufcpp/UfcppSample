using System;

class Program
{
    static void Main()
    {
    }
}

namespace UsingStandard
{
    using System.Linq;

    class Sample
    {
        public static void Run()
        {
            var x = new[] { 1, 2, 3, 4, 5 };
            var y = x.Where(i => (i & 1) != 0).Select(i => i * i);
            Console.WriteLine(string.Join(", ", y));
        }
    }
}

namespace UsingBackport
{
    extern alias Backport;
    using Backport::System.Linq;

    class Sample
    {
        public static void Run()
        {
            var x = new[] { 1, 2, 3, 4, 5 };
            var y = x.Where(i => (i & 1) != 0).Select(i => i * i);
            Console.WriteLine(string.Join(", ", y));
        }
    }
}
