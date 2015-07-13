using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Sort
{
    partial class Sort
    {
        /// <summary>
        /// 基数ソート。
        /// 概念説明用の簡易版。
        /// 10進数で3桁(0～999)までしかソートできない。
        /// </summary>
        /// <param name="a">対象の配列</param>
        /// <param name="max">配列 a 中の最大値</param>
        public static void RadixSort10(int[] a)
        {
            // バケツを用意
            List<int>[] bucket = new List<int>[10];

            for (int d = 0, r = 1; d < 3; ++d, r *= 10)
            {
                // バケツに値を入れる
                for (int i = 0; i < a.Length; ++i)
                {
                    int key = (a[i] / r) % 10; // a[i] の d 桁目だけを取り出す。
                    if (bucket[key] == null) bucket[key] = new List<int>();
                    bucket[key].Add(a[i]);
                }

                // バケツ中の値の結合
                for (int j = 0, i = 0; j < bucket.Length; ++j)
                    if (bucket[j] != null)
                        foreach (int val in bucket[j])
                            a[i++] = val;

                // バケツを一度空にする
                for (int j = 0; j < bucket.Length; ++j)
                    bucket[j] = null;
            }
        }

        /// <summary>
        /// 基数ソート。
        /// </summary>
        /// <param name="a">対象の配列</param>
        /// <param name="max">配列 a 中の最大値</param>
        public static void RadixSort(int[] a)
        {
            // バケツを用意
            List<int>[] bucket = new List<int>[256];

            for (int d = 0, logR = 0; d < 4; ++d, logR += 8)
            {
                // バケツに値を入れる
                for (int i = 0; i < a.Length; ++i)
                {
                    int key = (a[i] >> logR) & 255; // a[i] を256進 d 桁目だけを取り出す。
                    if (bucket[key] == null) bucket[key] = new List<int>();
                    bucket[key].Add(a[i]);
                }

                // バケツ中の値の結合
                for (int j = 0, i = 0; j < bucket.Length; ++j)
                    if (bucket[j] != null)
                        foreach (int val in bucket[j])
                            a[i++] = val;

                // バケツを一度空にする
                for (int j = 0; j < bucket.Length; ++j)
                    bucket[j] = null;
            }
        }
    }
}
