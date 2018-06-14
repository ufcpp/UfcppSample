using System;
using System.Collections.Generic;
using System.Text;

namespace ByRef.RefReassignment.RefFor
{
    class Program
    {
        static void Main()
        {
            var array = new[] { 1, 3, 5, 2, 4 };

            var x = 0;

            for (ref int i = ref x; i < array.Length; i++)
            {
                if (array[i] == 5) break;
            }

            Console.WriteLine(x); // break した時点の i の値 = 2
        }
    }
}
