using System;

namespace FlowAnalysis.Default
{
    namespace Struct
    {
        class Program
        {
            struct S { public string Name; }
            static int M(S s) => s.Name.Length;
            static void Main() => M(default);
        }
    }

    namespace Array
    {
        class Program
        {
            static void Main()
            {
                var array = new string[1];
                Console.WriteLine(array[0].Length);
            }
        }
    }
}
