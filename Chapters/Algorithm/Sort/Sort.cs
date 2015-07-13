using System;

namespace Algorithm.Sort
{
    partial class Sort
    {
        private static void Swap<T>(ref T t1, ref T t2) where T : IComparable<T>
        {
            var t = t1;
            t1 = t2;
            t2 = t1;
        }
    }
}
