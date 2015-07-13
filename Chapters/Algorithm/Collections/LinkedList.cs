using System.Collections.Generic;

namespace Collections
{
  /// <summary>
  /// 双方向連結リスト。
  /// </summary>
  /// <typeparam name="T">要素の型</typeparam>
  public class LinkedList<T> : IEnumerable<T>
  {
    #region 内部クラス

    /// <summary>
    /// 連結リストのノード。
    /// </summary>
    public class Node
    {
      #region フィールド

      T val;
      Node prev;
      Node next;

      #endregion
      #region 初期化

      internal Node(T val, Node prev, Node next)
      {
        this.val = val;
        this.prev = prev;
        this.next = next;
      }

      #endregion
      #region プロパティ

      /// <summary>
      /// 格納している要素を取得。
      /// </summary>
      public T Value
      {
        get { return this.val; }
        set { this.val = value; }
      }

      /// <summary>
      /// 次のノード。
      /// </summary>
      public Node Next
      {
        get { return this.next; }
        internal set { this.next = value; }
      }

      /// <summary>
      /// 次のノード。
      /// </summary>
      public Node Previous
      {
        get { return this.prev; }
        internal set { this.prev = value; }
      }

      #endregion
    }

    #endregion
    #region フィールド

    Node dummy;

    #endregion
    #region 初期化

    public LinkedList()
    {
      this.dummy = new Node(default(T), null, null);
      this.dummy.Next = this.dummy;
      this.dummy.Previous = this.dummy;
    }

    #endregion
    #region プロパティ

    /// <summary>
    /// リストの先頭ノード。
    /// </summary>
    public Node First
    {
      get { return this.dummy.Next; }
    }

    /// <summary>
    /// リストの末尾ノード。
    /// </summary>
    public Node Last
    {
      get { return this.dummy.Previous; }
    }

    /// <summary>
    /// リストの終端（末尾よりも後ろの番兵に当たるノード）。
    /// </summary>
    public Node End
    {
      get { return this.dummy; }
    }

    /// <summary>
    /// 要素の個数。
    /// </summary>
    public int Count
    {
      get
      {
        int i = 0;
        for (Node n = this.First; n != this.End; n = n.Next)
          ++i;
        return i;
      }
    }

    #endregion
    #region 挿入・削除

    /// <summary>
    /// ノード n の後ろに新しい要素を追加。
    /// </summary>
    /// <param name="n">要素の挿入位置</param>
    /// <param name="elem">新しい要素</param>
    /// <returns>新しく挿入されたノード</returns>
    public Node InsertAfter(Node n, T elem)
    {
      Node m = new Node(elem, n, n.Next);
      n.Next.Previous = m;
      n.Next = m;
      return m;
    }

    /// <summary>
    /// ノード n の前に新しい要素を追加。
    /// </summary>
    /// <param name="n">要素の挿入位置</param>
    /// <param name="elem">新しい要素</param>
    /// <returns>新しく挿入されたノード</returns>
    public Node InsertBefore(Node n, T elem)
    {
      Node m = new Node(elem, n.Previous, n);
      n.Previous.Next = m;
      n.Previous = m;
      return m;
    }

    /// <summary>
    /// 先頭に新しい要素を追加。
    /// </summary>
    /// <param name="elem">新しい要素</param>
    /// <returns>新しく挿入されたノード</returns>
    public Node InsertFirst(T elem)
    {
      return this.InsertAfter(this.dummy, elem);
    }

    /// <summary>
    /// 末尾に新しい要素を追加。
    /// </summary>
    /// <param name="elem">新しい要素</param>
    /// <returns>新しく挿入されたノード</returns>
    public Node InsertLast(T elem)
    {
      return this.InsertBefore(this.dummy, elem);
    }

    /// <summary>
    /// ノード n の自身を削除。
    /// </summary>
    /// <param name="n">要素の削除位置</param>
    /// <returns>削除した要素の次のノード</returns>
    public Node Erase(Node n)
    {
      if (n == this.dummy)
      {
        return this.dummy;
      }
      n.Previous.Next = n.Next;
      n.Next.Previous = n.Previous;
      return n.Next;
    }

    /// <summary>
    /// 先頭の要素を削除。
    /// </summary>
    public void EraseFirst()
    {
      this.Erase(this.First);
    }

    /// <summary>
    /// 末尾の要素を削除。
    /// </summary>
    public void EraseLast()
    {
      this.Erase(this.Last);
    }

    #endregion
    #region IEnumerable<T> メンバ

    public IEnumerator<T> GetEnumerator()
    {
      for (Node n = this.First; n != this.End; n = n.Next)
        yield return n.Value;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    #endregion
  }
}
