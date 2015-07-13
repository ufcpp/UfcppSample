using System;

namespace Algorithm.Sort
{
    partial class Sort
    {
        /// <summary>
        /// 選択ソート。
        /// </summary>
        /// <param name="a">対象の配列</param>
        public static void SelectSort<T>(T[] a)
          where T : IComparable<T>
        {
            int n = a.Length;
            for (int i = 0; i < n; i++)
            {
                int min = i;
                for (int j = i + 1; j < n; j++)
                    if (a[min].CompareTo(a[j]) > 0)
                        min = j;
                Swap(ref a[i], ref a[min]);
            }
        }
    }
}
