namespace OverloadResolution.Generic
{
    using System;

    class Program
    {
        static void Main()
        {
            // M(string) の方が呼ばれる
            M("abc");

            // M<T>(string) の方が呼ばれる
            M<int>("abc");
        }

        static void M(string x) => Console.WriteLine("M");
        static void M<T>(string x) => Console.WriteLine("M<T>");
    }
}
