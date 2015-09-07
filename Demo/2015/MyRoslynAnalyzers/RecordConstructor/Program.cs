using System;

namespace RecordConstructor
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Sample(10, 20);

            Console.WriteLine($"({s.X}, {s.Y})");
/*
結果:

(10, 29)
*/
        }
    }
}
