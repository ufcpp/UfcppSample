using System;
using System.Text;

namespace Grisu3DoubleConversion
{
    class Program
    {
        static void Main(string[] args)
        {
            var r = new Random();
            var buffer = new byte[30];

            for (int i = 0; i < 100; i++)
            {
                var x = r.NextDouble() * Math.Pow(10, r.Next(-20, 20));

                {
                    DoubleConversion.ToString(x, false, buffer, out var len, out var exp);
                    var s = Encoding.ASCII.GetString(buffer, 0, len);
                    Console.WriteLine((x, s, len, exp));
                }
                {
                    DoubleConversion.ToString(x, true, buffer, out var len, out var exp);
                    var s = Encoding.ASCII.GetString(buffer, 0, len);
                    Console.WriteLine(((float)x, s, len, exp));
                }
                Console.WriteLine();
            }
        }
    }
}
