using System;
using System.Collections.Generic;
using System.Text;

namespace Collections
{
  /// <summary>
  /// 優先度付き待ち行列
  /// </summary>
  /// <typeparam name="T">要素の型</typeparam>
  class PriorityQueue<T>
    where T : IComparable<T>
  {
    #region フィールド

    ArrayList<T> buffer;

    #endregion
    #region 初期化

    public PriorityQueue() { this.buffer = new ArrayList<T>(); }
    public PriorityQueue(int capacity) { this.buffer = new ArrayList<T>(capacity); }

    #endregion
    #region ヒープ操作

    /// <summary>
    /// ヒープ化されている配列リストに新しい要素を追加する。
    /// </summary>
    /// <param name="array">対象の配列リスト</param>
    static void PushHeap(ArrayList<T> array, T elem)
    {
      int n = array.Count;
      array.InsertLast(elem);

      while (n != 0)
      {
        int i = (n - 1) / 2;
        if (array[n].CompareTo(array[i]) > 0)
        {
          T tmp = array[n]; array[n] = array[i]; array[i] = tmp;
        }
        n = i;
      }
    }

    /// <summary>
    /// ヒープから最大値を削除する。
    /// </summary>
    /// <param name="array">対象の配列リスト</param>
    static void PopHeap(ArrayList<T> array)
    {
      int n = array.Count - 1;
      array[0] = array[n];
      array.EraseLast();

      for (int i = 0, j; (j = 2 * i + 1) < n; )
      {
        if ((j != n - 1) && (array[j].CompareTo(array[j + 1]) < 0))
          j++;
        if (array[i].CompareTo(array[j]) < 0)
        {
          T tmp = array[j]; array[j] = array[i]; array[i] = tmp;
        }
        i = j;
      }
    }

    #endregion
    #region 要素の挿入・削除

    /// <summary>
    /// 要素のプッシュ。
    /// </summary>
    /// <param name="elem">挿入したい要素</param>
    public void Push(T elem)
    {
      PushHeap(this.buffer, elem);
    }

    /// <summary>
    /// 要素を1つポップ。
    /// </summary>
    /// <remarks>
    /// 今回の実装では、先頭要素の読み出しと削除は別に行う。
    /// この Pop では削除のみ。
    /// 読み出しには Top プロパティを使う。
    /// </remarks>
    public void Pop()
    {
      PopHeap(this.buffer);
    }

    /// <summary>
    /// 先頭要素の読み出し。
    /// </summary>
    public T Top
    {
      get { return this.buffer[0]; }
    }

    public int Count
    {
      get { return this.buffer.Count; }
    }

    #endregion
  }
}
