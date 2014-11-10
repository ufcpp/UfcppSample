using System;

namespace VersionSample.Csharp4
{
    public class DefaultParameterSample
    {
        public static void Run()
        {
            Console.WriteLine(Sum(1, 2, 3));
            Console.WriteLine(Sum(x: 1, y: 2, z: 3));
            Console.WriteLine(Sum(y: 2, z: 3));
            Console.WriteLine(Sum(x: 1, z: 3));
            Console.WriteLine(Sum(x: 1, y: 2));
            Console.WriteLine(Sum(x: 1));
            Console.WriteLine(Sum(y: 2));
            Console.WriteLine(Sum(z: 3));
            Console.WriteLine(Sum());
        }

        public static int Sum(int x = 0, int y = 0, int z = 0)
        {
            return x + y + z;
        }
    }
}
