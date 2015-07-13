using System;
using System.Collections.Generic;

namespace Collections
{
  /// <summary>
  /// ソート済み配列。
  /// </summary>
  /// <typeparam name="T">要素の型</typeparam>
  /// <remarks>
  /// 2分探索できるので、検索は O(log n)。
  /// ソート済みの状態を保つため、要素の挿入・削除には O(n) 必要。
  /// </remarks>
  class SortedArray<T> : ISet<T>
    where T: IComparable<T>
  {
    #region フィールド

    ArrayList<T> buffer;

    #endregion
    #region 初期化

    public SortedArray() : this(256) { }

    public SortedArray(int capacity)
    {
      this.buffer = new ArrayList<T>(capacity);
    }

    #endregion
    #region 要素の挿入・削除・検索

    /// <summary>
    /// 要素の挿入。
    /// </summary>
    /// <param name="elem">挿入する要素</param>
    public void Insert(T elem)
    {
      if (this.buffer.Count == 0)
      {
        this.buffer.InsertLast(elem);
        return;
      }

      int r = this.buffer.Count - 1;
      int l = 0;
      int comp;
      while (l < r)
      {
        int m = (r + l) >> 1;
        comp = this.buffer[m].CompareTo(elem);
        if (comp > 0) r = m - 1;
        else if (comp < 0) l = m + 1;
        else return; // 重複不可
      }

      comp = this.buffer[l].CompareTo(elem);
      if(comp < 0)
        this.buffer.Insert(l + 1, elem);
      else if(comp > 0)
        this.buffer.Insert(l, elem);
    }

    /// <summary>
    /// 要素の検索。
    /// </summary>
    /// <param name="elem">検索する要素</param>
    /// <returns>要素の位置（見つからなかった場合、配列長）</returns>
    public int IndexOf(T elem)
    {
      if (this.buffer.Count == 0)
        return 0;

      int r = this.buffer.Count - 1;
      int l = 0;
      while (l < r)
      {
        int m = (r + l) >> 1;
        int comp = this.buffer[m].CompareTo(elem);
        if (comp > 0) r = m - 1;
        else if (comp < 0) l = m + 1;
        else return m;
      }

      if(this.buffer[l].CompareTo(elem) == 0)
        return l;
      return this.buffer.Count;
    }

    /// <summary>
    /// テーブル中に要素が含まれているかどうか判別。
    /// </summary>
    /// <param name="elem">探したい要素</param>
    /// <returns>含まれていれば true</returns>
    public bool Contains(T elem)
    {
      return this.IndexOf(elem) != this.buffer.Count;
    }

    /// <summary>
    /// テーブル中の要素を検索。
    /// </summary>
    /// <param name="elem">探したい要素</param>
    /// <returns>含まれていればその要素を、なければ default(T)</returns>
    public T Find(T elem)
    {
      int i = this.IndexOf(elem);
      if (i == this.buffer.Count)
        return default(T);
      return this.buffer[i];
    }

    /// <summary>
    /// 要素の削除。
    /// </summary>
    /// <param name="elem">削除する要素</param>
    public void Erase(T elem)
    {
      int i = this.IndexOf(elem);
      if (i < this.buffer.Count)
        this.buffer.Erase(i);
    }

    #endregion
    #region IEnumerable<T> メンバ

    public IEnumerator<T> GetEnumerator()
    {
      for (int i = 0; i < this.buffer.Count; ++i)
        yield return this.buffer[i];
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    #endregion
  }
}
