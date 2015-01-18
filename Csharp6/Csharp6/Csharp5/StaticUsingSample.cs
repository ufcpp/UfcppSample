namespace Csharp6.Csharp5.StaticUsing
{
    using System;

    class Program
    {
        static void Main()
        {
            var pi = 2 * Math.Asin(1);
            Console.WriteLine(Math.PI == pi);
        }
    }
}
