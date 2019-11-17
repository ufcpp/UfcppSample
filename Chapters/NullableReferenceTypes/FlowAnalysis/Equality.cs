using System;
using System.Collections.Generic;

namespace FlowAnalysis.Equality
{
    class Program
    {
        static void Main()
        {
            Equality("", null);
            Equality("", "");
        }

        private static void Equality(string x, string? y)
        {
            if (x == y)
            {
                Console.WriteLine(y.Length);
            }
            else
            {
                Console.WriteLine(y.Length);
            }
        }

        private static void SpecialEquality(string x, string? y)
        {
            if (EqualityComparer<string>.Default.Equals(x, y))
            {
                Console.WriteLine(y.Length);
            }
            else
            {
                Console.WriteLine(y.Length);
            }
        }
    }
}
