using System;

namespace Algorithm.Sort
{
    partial class Sort
    {
        /// <summary>
        /// ヒープソート。
        /// </summary>
        /// <param name="a">対象の配列</param>
        public static void HeapSort<T>(T[] a)
          where T : IComparable<T>
        {
            for (int i = 1; i < a.Length; ++i)
                MakeHeap(a, i);
            for (int i = a.Length - 1; i >= 0; --i)
                a[i] = PopHeap(a, i);
        }

        /// <summary>
        /// 配列をヒープ化する。
        /// n - 1 番目までの要素は既にヒープ化されていることを仮定して、
        /// n 番目の要素をヒープに追加。
        /// </summary>
        /// <param name="a">対象の配列</param>
        /// <param name="n">要素数</param>
        static void MakeHeap<T>(T[] a, int n)
          where T : IComparable<T>
        {
            while (n != 0)
            {
                int i = (n - 1) / 2;
                if (a[n].CompareTo(a[i]) > 0) Swap(ref a[n], ref a[i]);
                n = i;
            }
        }

        /// <summary>
        /// ヒープから最大値を取り出す。
        /// </summary>
        /// <param name="a">対象の配列</param>
        /// <param name="n">要素数 - 1</param>
        /// <returns>取り出した最大値</returns>
        static T PopHeap<T>(T[] a, int n)
          where T : IComparable<T>
        {
            T max = a[0];

            a[0] = a[n];

            for (int i = 0, j; (j = 2 * i + 1) < n;)
            {
                if ((j != n - 1) && (a[j].CompareTo(a[j + 1]) < 0)) j++;
                if (a[i].CompareTo(a[j]) < 0) Swap(ref a[i], ref a[j]);
                i = j;
            }

            return max;
        }
    }
}
