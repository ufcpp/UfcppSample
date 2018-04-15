namespace OverloadResolution.StringInterpolation
{
    using System;

    class Program
    {
        static void Main()
        {
            var (a, b) = (1, 2);

            // M(string) の方が呼ばれる
            M($"{a}, {b}");

            // こう書けば M(FormattableString) の方
            M((FormattableString)$"{a}, {b}");
        }

        static void M(string x) => Console.WriteLine("string");
        static void M(FormattableString x) => Console.WriteLine("FormattableString");
    }
}
