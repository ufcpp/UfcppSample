using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            { int    x = 0;   AvoidBoxing.Set1(ref x); Console.WriteLine(x); }
            { double x = 0;   AvoidBoxing.Set1(ref x); Console.WriteLine(x); }
            { char   x = '0'; AvoidBoxing.Set1(ref x); Console.WriteLine(x); }
            { string x = "0"; AvoidBoxing.Set1(ref x); Console.WriteLine(x); }
            { long   x = 0;   AvoidBoxing.Set1(ref x); Console.WriteLine(x); }

            { int    x = 0;   Boxed.Set1(ref x); Console.WriteLine(x); }
            { double x = 0;   Boxed.Set1(ref x); Console.WriteLine(x); }
            { char   x = '0'; Boxed.Set1(ref x); Console.WriteLine(x); }
            { string x = "0"; Boxed.Set1(ref x); Console.WriteLine(x); }
            { long   x = 0;   Boxed.Set1(ref x); Console.WriteLine(x); }
        }
    }

    class AvoidBoxing
    {
        public static void Set1<T>(ref T value)
        {
            if (value is int) __refvalue(__makeref(value), int) = 1;
            else if (value is double) __refvalue(__makeref(value), double) = 1;
            else if (value is char  ) __refvalue(__makeref(value), char  ) = '1';
            else if (value is string) __refvalue(__makeref(value), string) = "1";
            else value = default(T);
        }
    }

    class Boxed
    {
        public static void Set1<T>(ref T value)
        {
            // 型を見て分岐しているのに、結局一度 (T)(object) とキャストしないと行けない
            // (object)の時点でボックス化発生
            if (value is int) value = (T)(object)1;
            else if (value is double) value = (T)(object)1.0;
            else if (value is char  ) value = (T)(object)'1';
            else if (value is string) value = (T)(object)"1";
            else value = default(T);
        }
    }
}
