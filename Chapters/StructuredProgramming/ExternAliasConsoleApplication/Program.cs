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
            var y = x.Where(i => (i & 1) != 0).Select(i => i * i); // 標準の LINQ
            Console.WriteLine(string.Join(", ", y));
        }
    }
}

namespace UsingBackport
{
    extern alias Backport; // コンパイル オプションで BackportEnumerable.dll を指定
    using Backport::System.Linq;

    class Sample
    {
        public static void Run()
        {
            var x = new[] { 1, 2, 3, 4, 5 };
            var y = x.Where(i => (i & 1) != 0).Select(i => i * i); // 自作のパックポート LINQ
            Console.WriteLine(string.Join(", ", y));
        }
    }
}
