using System;

namespace Algorithm.Sort
{
    partial class Sort
    {
        /// <summary>
        /// マージソート。
        /// </summary>
        /// <param name="a">対象の配列</param>
        public static void MergeSort<T>(T[] a)
          where T : IComparable<T>
        {
            T[] work = new T[a.Length / 2];
            MergeSort(a, 0, a.Length, work);
        }

        /// <summary>
        /// マージソート → 挿入ソートに切り替える配列長の閾値。
        /// </summary>
        const int MERGE_THREASHOLD = 64;

        /// <summary>
        /// マージソート。
        /// 配列のどこからどこまでをソートするかを指定するバージョン。
        /// </summary>
        /// <param name="a">対象の配列</param>
        /// <param name="begin">ソート対象部分の先頭</param>
        /// <param name="end">ソート対象部分の末尾＋1</param>
        /// <param name="work">作業領域。a の 1/2 のサイズが必要。</param>
        static void MergeSort<T>(T[] a, int begin, int end, T[] work)
          where T : IComparable<T>
        {
            if (end - begin < MERGE_THREASHOLD)
            {
                InsertSort(a, begin, end - 1);
                return;
            }

            int mid = (begin + end) / 2;
            MergeSort(a, begin, mid, work);
            MergeSort(a, mid, end, work);
            Merge(a, begin, mid, end, work);
        }

        /// <summary>
        /// 配列 a の、[begin, mid) の部分と [mid, end) の部分をマージ。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">マージ対象の配列</param>
        /// <param name="begin1">aの先頭</param>
        /// <param name="mid">aの分割点</param>
        /// <param name="end">aの末尾＋1</param>
        /// <param name="work">作業領域</param>
        static void Merge<T>(T[] a, int begin, int mid, int end, T[] work)
          where T : IComparable<T>
        {
            int i, j, k;

            for (i = begin, j = 0; i != mid; ++i, ++j) work[j] = a[i];

            mid -= begin;
            for (j = 0, k = begin; i != end && j != mid; ++k)
            {
                if (a[i].CompareTo(work[j]) < 0)
                {
                    a[k] = a[i];
                    ++i;
                }
                else
                {
                    a[k] = work[j];
                    ++j;
                }
            }

            for (; i < end; ++i, ++k) a[k] = a[i];
            for (; j < mid; ++j, ++k) a[k] = work[j];
        }
    }
}
