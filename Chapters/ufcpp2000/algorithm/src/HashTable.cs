using System;
using System.Collections.Generic;
using System.Text;

namespace Collections
{
  /// <summary>
  /// ハッシュテーブル。
  /// </summary>
  /// <typeparam name="T">要素の型</typeparam>
  class HashTable<T> : ISet<T>
  {
    #region 内部クラス

    class Node
    {
      internal T val;
      internal Node next;

      internal Node(T val, Node next)
      {
        this.val = val;
        this.next = next;
      }
    }

    #endregion
    #region フィールド

    Node[] table;
    int mask;

    #endregion
    #region 初期化

    public HashTable() : this(256) { }

    public HashTable(int capacity)
    {
      capacity = Pow2((uint)capacity);
      this.mask = capacity - 1;
      this.table = new Node[capacity];
    }

    static int Pow2(uint n)
    {
      --n;
      int p = 0;
      for (; n != 0; n >>= 1) p = (p << 1) + 1;
      return p + 1;
    }

    #endregion
    #region 挿入・削除・検索

    /// <summary>
    /// 要素の挿入。
    /// </summary>
    /// <param name="elem">挿入する要素</param>
    public void Insert(T elem)
    {
      int code = elem.GetHashCode() & this.mask;
      Node n = this.table[code];
      Node m = new Node(elem, n);
      m.next = n;
      this.table[code] = m;
    }

    /// <summary>
    /// 要素の削除。
    /// </summary>
    /// <param name="elem">削除する要素</param>
    public void Erase(T elem)
    {
      int code = elem.GetHashCode() & this.mask;
      Node n = this.table[code];

      if (n == null) return;
      if (n.next == null)
        this.table[code] = null;

      while (n.next != null && n.next.val.Equals(elem)) n = n.next;
      if(n.next != null)
        n.next = n.next.next;
    }

    /// <summary>
    /// テーブル中に要素が含まれているかどうか判別。
    /// </summary>
    /// <param name="elem">探したい要素</param>
    /// <returns>含まれていれば true</returns>
    public bool Contains(T elem)
    {
      int code = elem.GetHashCode() & this.mask;
      Node n = this.table[code];
      while (n != null && !n.val.Equals(elem)) n = n.next;
      return n != null;
    }

    /// <summary>
    /// テーブル中の要素を検索。
    /// </summary>
    /// <param name="elem">探したい要素</param>
    /// <returns>含まれていればその要素を、なければ default(T)</returns>
    public T Find(T elem)
    {
      int code = elem.GetHashCode();
      Node n = this.table[code & this.mask];
      while (n != null && !n.val.Equals(elem)) n = n.next;
      if (n == null) return default(T);
      return n.val;
    }

    #endregion
    #region IEnumerable<T> メンバ

    public IEnumerator<T> GetEnumerator()
    {
      for (int i = 0; i < this.table.Length; ++i)
        for (Node n = this.table[i]; n != null; n = n.next)
          yield return n.val;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    #endregion
  }
}
