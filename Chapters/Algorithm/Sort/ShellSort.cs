using System;

namespace Algorithm.Sort
{
    partial class Sort
    {
        /// <summary>
        /// シェルソート。
        /// </summary>
        /// <param name="a">対象の配列</param>
        public static void ShellSort<T>(T[] a)
          where T : IComparable<T>
        {
            int n = a.Length;
            int h;
            for (h = 1; h < n / 9; h = h * 3 + 1) ;
            for (; h > 0; h /= 3)
                for (int i = h; i < n; i++)
                    for (int j = i; j >= h && a[j - h].CompareTo(a[j]) > 0; j -= h)
                        Swap(ref a[j], ref a[j - h]);
        }
    }
}
