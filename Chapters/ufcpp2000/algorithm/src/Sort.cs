using System;
using System.Collections.Generic;

static class Sort
{
  #region common

  /// <summary>
  /// a と b の中身を入れ替える。
  /// </summary>
  /// <param name="a">オペランドa</param>
  /// <param name="b">オペランドb</param>
  public static void Swap<T>(ref T a, ref T b)
  {
    T c = a; a = b; b = c;
  }

  #endregion
  #region O(n^2) ソート

  /// <summary>
  /// バブルソート。
  /// </summary>
  /// <param name="a">対象の配列</param>
  public static void BubbleSort<T>(T[] a)
    where T : IComparable<T>
  {
    int n = a.Length;
    for (int i = 0; i < n - 1; i++)
      for (int j = n - 1; j > i; j--)
        if (a[j].CompareTo(a[j - 1]) < 0)
          Swap(ref a[j], ref a[j - 1]);
  }

  /// <summary>
  /// 選択ソート。
  /// </summary>
  /// <param name="a">対象の配列</param>
  public static void SelectSort<T>(T[] a)
    where T : IComparable<T>
  {
    int n = a.Length;
    for (int i = 0; i < n; i++)
      for (int j = 1; j < n - i; j++)
        if (a[j - 1].CompareTo(a[j]) > 0)
          Swap(ref a[j - 1], ref a[j]);
  }

  /// <summary>
  /// 挿入ソート。
  /// </summary>
  /// <param name="a">対象の配列</param>
  public static void InsertSort<T>(T[] a)
    where T : IComparable<T>
  {
    int n = a.Length;
    for (int i = 1; i < n; i++)
      for (int j = i; j >= 1 && a[j - 1].CompareTo(a[j]) > 0; --j )
        Swap(ref a[j], ref a[j - 1]);
  }

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

  #endregion
  #region クイックソート・マージソート準備

  /// <summary>
  /// クイックソートやマージソート → 挿入ソートに切り替える配列長の閾値。
  /// </summary>
  const int THREASHOLD = 64;

  /// <summary>
  /// 挿入ソート。
  /// 配列のどこからどこまでをソートするかを指定するバージョン。
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

  #endregion
  #region クイックソート

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
    if (last - first < THREASHOLD)
    {
      InsertSort(a, first, last);
      return;
    }

    // 枢軸決定（配列の先頭、ど真ん中、末尾の3つの値の中央値を使用。）
    T pivot = Median(a[first], a[(first + last) / 2], a[last]);

    // 左右分割
    int l = first;
    int r = last;

	  while(l <= r)
	  {
      while (l < last && a[l].CompareTo(pivot) < 0) l++;
      while (r > first && a[r].CompareTo(pivot) >= 0) r--;
      if (l > r) break;
      Swap(ref a[l], ref a[r]);
      l++; r--;
    }

    // 再帰呼び出し
    QuickSort(a, first, l-1);
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

  #endregion
  #region ヒープソート

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

    for (int i=0, j; (j = 2 * i + 1) < n; )
    {
      if ((j != n - 1) && (a[j].CompareTo(a[j + 1]) < 0)) j++;
      if (a[i].CompareTo(a[j]) < 0) Swap(ref a[i], ref a[j]);
      i = j;
    }

    return max;
  }

  #endregion
  #region マージソート

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
    if (end - begin < THREASHOLD)
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

  #endregion
  #region バケットソート

  /// <summary>
  /// [0, max] の範囲の整数をバケットソート。
  /// </summary>
  /// <param name="a">対象の配列</param>
  /// <param name="max">配列 a 中の最大値</param>
  public static void BucketSort(int[] a, int max)
  {
    // バケツを用意
    int[] bucket = new int[max + 1];

    // バケツに値を入れる
    for (int i = 0; i < a.Length; ++i) ++bucket[a[i]];

    // バケツ中の値の結合
    for (int j = 0, i = 0; j < bucket.Length; ++j)
      for (int k = bucket[j]; k != 0; --k, ++i)
        a[i] = j;
  }

  /// <summary>
  /// [0, max] の範囲の整数をキーに持つデータ構造をバケットソート。
  /// </summary>
  /// <typeparam name="T">値の型</typeparam>
  /// <param name="a">対象の配列</param>
  /// <param name="max">キーの最大値</param>
  public static void BucketSort<T>(KeyValuePair<int, T>[] a, int max)
  {
    // バケツを用意
    List<T>[] bucket = new List<T>[max + 1];

    // バケツに値を入れる
    for (int i = 0; i < a.Length; ++i)
    {
      if (bucket[a[i].Key] == null) bucket[a[i].Key] = new List<T>();
      bucket[a[i].Key].Add(a[i].Value);
    }

    // バケツ中の値の結合
    for (int j = 0, i = 0; j < bucket.Length; ++j)
     if(bucket[j] != null)
       foreach (T val in bucket[j])
         a[i++] = new KeyValuePair<int, T>(j, val);
  }

  #endregion
  #region 基数ソート

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

  #endregion
}
