using System.Collections.Generic;

namespace Collections
{
  /// <summary>
  /// 片方向連結リスト。
  /// </summary>
  /// <typeparam name="T">要素の型</typeparam>
  public class ForwardLinkedList<T> : IEnumerable<T>
  {
    #region 内部クラス

    /// <summary>
    /// 片方向連結リストのノード。
    /// </summary>
    public class Node
    {
      #region フィールド

      T val;
      Node next;

      #endregion
      #region 初期化

      internal Node(T val, Node next)
      {
        this.val = val;
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

      #endregion
    }

    #endregion
    #region フィールド

    Node first;

    #endregion
    #region 初期化

    public ForwardLinkedList()
    {
      this.first = null;
    }

    #endregion
    #region プロパティ

    /// <summary>
    /// リストの先頭ノード。
    /// </summary>
    public Node First
    {
      get { return this.first; }
    }

    /// <summary>
    /// 要素の個数。
    /// </summary>
    public int Count
    {
      get
      {
        int i = 0;
        for(Node n = this.first; n!=null; n=n.Next)
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
      Node m = new Node(elem, n.Next);
      n.Next = m;
      return m;
    }

    /// <summary>
    /// 先頭に新しい要素を追加。
    /// </summary>
    /// <param name="elem">新しい要素</param>
    /// <returns>新しく挿入されたノード</returns>
    public Node InsertFirst(T elem)
    {
      Node m = new Node(elem, this.first);
      this.first = m;
      return m;
    }

    /// <summary>
    /// ノード n の後ろの要素を削除。
    /// </summary>
    /// <param name="n">要素の削除位置</param>
    public void EraseAfter(Node n)
    {
      if (n != null && n.Next != null)
        n.Next = n.Next.Next;
    }

    /// <summary>
    /// ノード n の自身を削除。
    /// </summary>
    /// <param name="n">要素の削除位置</param>
    /// <returns>削除した要素の次のノード</returns>
    public Node Erase(Node n)
    {
      Node prev = this.first;
      for (; prev != null && prev.Next != n; prev = prev.Next) ;
      if (prev == this.first)
      {
        this.first = null;
        return null;
      }
      if (prev != null)
      {
        this.EraseAfter(prev);
        return prev.Next;
      }
      return null;
    }

    /// <summary>
    /// 先頭の要素を削除。
    /// </summary>
    public void EraseFirst()
    {
      if (this.first != null)
        this.first = this.first.Next;
    }

    #endregion
    #region IEnumerable<T> メンバ

    public IEnumerator<T> GetEnumerator()
    {
      for (Node n = this.first; n != null; n = n.Next)
        yield return n.Value;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    #endregion
  }
}
