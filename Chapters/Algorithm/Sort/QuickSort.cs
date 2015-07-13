using System;

namespace Algorithm.Sort
{
    partial class Sort
    {
        /// <summary>
        /// クイックソート。
        /// </summary>
        /// <param name="a">対象の配列</param>
        public static void QuickSort<T>(T[] a)
          where T : IComparable<T>
        {
            QuickSort(a, 0, a.Length - 1);
        }

        /// <summary>
        /// クイックソート → 挿入ソートに切り替える配列長の閾値。
        /// </summary>
        const int QUICK_THREASHOLD = 64;

        /// <summary>
        /// クイックソート本体。
        /// 配列のどこからどこまでをソートするかを指定するバージョン。
        /// </summary>
        /// <param name="a">対象の配列</param>
        /// <param name="first">ソート対象の先頭インデックス</param>
        /// <param name="last">ソート対象の末尾インデックス</param>
        static void QuickSort<T>(T[] a, int first, int last)
          where T : IComparable<T>
        {
            // 要素数が少なくなってきたら挿入ソートに切り替え
            if (last - first < QUICK_THREASHOLD)
            {
                InsertSort(a, first, last);
                return;
            }

            // 枢軸決定（配列の先頭、ど真ん中、末尾の3つの値の中央値を使用。）
            T pivot = Median(a[first], a[(first + last) / 2], a[last]);

            // 左右分割
            int l = first;
            int r = last;

            while (l <= r)
            {
                while (l < last && a[l].CompareTo(pivot) < 0) l++;
                while (r > first && a[r].CompareTo(pivot) >= 0) r--;
                if (l > r) break;
                Swap(ref a[l], ref a[r]);
                l++; r--;
            }

            // 再帰呼び出し
            QuickSort(a, first, l - 1);
            QuickSort(a, l, last);
        }

        /// <summary>
        /// 3つの値の中央値を求める。
        /// </summary>
        /// <param name="a">オペランドa</param>
        /// <param name="b">オペランドb</param>
        /// <param name="c">オペランドc</param>
        /// <returns>中央値</returns>
        static T Median<T>(T a, T b, T c)
          where T : IComparable<T>
        {
            if (a.CompareTo(b) > 0) Swap(ref a, ref b);
            if (a.CompareTo(c) > 0) Swap(ref a, ref c);
            if (b.CompareTo(c) > 0) Swap(ref b, ref c);
            return b;
        }
    }
}
