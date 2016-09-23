namespace Keywords.NameOf.UsingStatic
{
    using System;
    using static MyExtensions;

    class Program
    {
        static void Main()
        {
            // 一見、nameof メソッドはなさそうに見えるけども…
            // using static MyExtensions; のせいで、MyExtensions.nameof が参照される
            var x = 1;
            Console.WriteLine(nameof(x)); // abc
        }
    }

    static class MyExtensions
    {
        public static string nameof(object x) => "abc";
    }
}
