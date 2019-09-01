using System;

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
    }
}
