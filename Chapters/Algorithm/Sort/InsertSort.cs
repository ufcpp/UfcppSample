using System;

namespace Algorithm.Sort
{
    partial class Sort
    {
        /// <summary>
        /// 挿入ソート。
        /// </summary>
        /// <param name="a">対象の配列</param>
        public static void InsertSort<T>(T[] a)
          where T : IComparable<T>
        {
            int n = a.Length;
            for (int i = 1; i < n; i++)
                for (int j = i; j >= 1 && a[j - 1].CompareTo(a[j]) > 0; --j)
                    Swap(ref a[j], ref a[j - 1]);
        }

        /// <summary>
        /// 挿入ソート。
        /// 配列のどこからどこまでをソートするかを指定するバージョン。
        /// <see cref="MergeSort{T}(T[])"/>, <see cref="QuickSort{T}(T[])"/>
        /// </summary>
        /// <param name="a">対象の配列</param>
        /// <param name="first">ソート対象の先頭インデックス</param>
        /// <param name="last">ソート対象の末尾インデックス</param>
        static void InsertSort<T>(T[] a, int first, int last)
          where T : IComparable<T>
        {
            for (int i = first + 1; i <= last; i++)
                for (int j = i; j > first && a[j - 1].CompareTo(a[j]) > 0; --j)
                    Swap(ref a[j], ref a[j - 1]);
        }
    }
}
