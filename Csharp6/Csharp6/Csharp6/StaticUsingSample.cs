namespace Csharp6.Csharp6.StaticUsing
{
    using System;
    using static System.Math;

    class Program
    {
        static void Main()
        {
            var pi = 2 * Asin(1);
            Console.WriteLine(PI == pi);
        }
    }
}
