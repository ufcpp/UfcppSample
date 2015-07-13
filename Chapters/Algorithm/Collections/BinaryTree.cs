using System;
using System.Collections.Generic;
using System.Text;

namespace Collections
{
  /// <summary>
  /// 2分探索木。
  /// </summary>
  /// <typeparam name="T">要素の型</typeparam>
  class BinaryTree<T> : ISet<T>
    where T: IComparable<T>
  {
    #region 内部クラス

    /// <summary>
    /// 2文探索木のノード。
    /// </summary>
    public class Node
    {
      #region フィールド

      internal T val;
      internal Node left, right, parent;

      #endregion
      #region 初期化

      internal Node() : this(default(T), null) { }

      internal Node(T val, Node parent)
      {
        this.val = val;
        this.parent = parent;
        this.left = this.right = null;
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
      /// このノードの次のノードを返す。
      /// （要素の値の小さい順にノードをたどる。）
      /// </summary>
      public Node Next
      {
        get
        {
          Node n = this;

          if (n.right != null)
            return n.right.Min;

          for (; n.parent != null && n.parent.left != n; n = n.parent) ;
          return n.parent;
        }
      }

      /// <summary>
      /// このノードの前のノードを返す。
      /// （要素の値の小さい順にノードをたどる。）
      /// </summary>
      public Node Previous
      {
        get
        {
          Node n = this;

          if (n.left != null)
            return n.left.Max;

          for (; n.parent != null && n.parent.right != n; n = n.parent) ;
          return n.parent;
        }
      }

      /// <summary>
      /// このノード以下の部分木中で、最小の要素を持つノード（＝左端ノード）を返す。
      /// </summary>
      internal Node Min
      {
        get
        {
          Node n = this;
          for (; n.left != null; n = n.left) ;
          return n;
        }
      }

      /// <summary>
      /// このノード以下の部分木中で、最大の要素を持つノード（＝右端ノード）を返す。
      /// </summary>
      internal Node Max
      {
        get
        {
          Node n = this;
          for (; n.right != null; n = n.right) ;
          return n;
        }
      }

      #endregion
      #region デバッグ用

      [System.Diagnostics.Conditional("DEBUG")]
      virtual public void Output(System.IO.TextWriter writer, int level)
      {
        for (int i = 0; i < level; ++i)
          writer.Write("\t");
        writer.Write("{0}\n", this.val);

        ++level;
        if (this.left != null) this.left.Output(writer, level);
        else
        {
          for (int i = 0; i < level; ++i)
            writer.Write("\t");
          writer.Write("null\n");
        }
        if (this.right != null) this.right.Output(writer, level);
        else
        {
          for (int i = 0; i < level; ++i)
            writer.Write("\t");
          writer.Write("null\n");
        }
      }

      #endregion
    }

    #endregion
    #region フィールド

    /// <summary>
    /// 根ノード。
    /// </summary>
    Node root;

    #endregion
    #region 初期化

    public BinaryTree()
    {
      this.root = null;
    }

    #endregion
    #region プロパティ

    /// <summary>
    /// 木構造を逐次探索する際の始点。
    /// </summary>
    public Node Begin
    {
      get
      {
        if (this.root == null)
          return this.End;
        return this.root.Min;
      }
    }

    /// <summary>
    /// 木構造を逐次探索する際の終端（末尾よりも後ろの番兵に当たるノード）。
    /// </summary>
    public Node End
    {
      get { return null; }
    }

    #endregion
    #region 要素の挿入・削除・検索

    /// <summary>
    /// 新しい要素を挿入する。
    /// </summary>
    /// <param name="elem">新しい要素</param>
    /// <returns>新しい要素を格納するノード</returns>
    public void Insert(T elem)
    {
      if (this.root == null)
      {
        this.root = new Node(elem, null);
        return;
      }

      Node n = this.root;
      Node p = null;
      while (n != null)
      {
        p = n;
        if (n.val.CompareTo(elem) > 0) n = n.left;
        else n = n.right;
      }

      n = new Node(elem, p);
      if (p.val.CompareTo(elem) > 0) p.left = n;
      else p.right = n;
    }

    /// <summary>
    /// n の片方の子は null、もう片方の子は m という前提の元で、
    /// ノード n の位置を子ノード m で置き換える。
    /// </summary>
    /// <param name="n">削除するノード</param>
    /// <param name="m">置き換える子ノード</param>
    void Replace(Node n, Node m)
    {
      Node p = n.parent;
      if (m != null) m.parent = p;
      if (n == this.root) this.root = m;
      else if (p.left == n) p.left = m;
      else p.right = m;
    }

    /// <summary>
    /// ノード n を削除する。
    /// </summary>
    /// <param name="n">削除したいノード</param>
    public void Erase(Node n)
    {
      if (n == null) return;

      if (n.left == null) this.Replace(n, n.right);
      else if(n.right == null) this.Replace(n, n.left);
      else
      {
        Node m = n.right.Min;
        n.Value = m.Value;
        this.Replace(m, m.right);
      }
    }

    /// <summary>
    /// 要素を削除する。
    /// </summary>
    /// <param name="elem">削除したい要素</param>
    public void Erase(T elem)
    {
      this.Erase(this.Find(elem));
    }

    /// <summary>
    /// ある値を持つノードを検索。
    /// (同じ値が複数ある場合、最初のノード。)
    /// 見つからなかった場合は null を返す。
    /// </summary>
    /// <param name="elem">探したい要素</param>
    /// <returns>見つけたノード</returns>
    public Node Find(T elem)
    {
      Node n = this.root;
      while (n != null)
      {
        if (n.val.CompareTo(elem) > 0) n = n.left;
        else if (n.val.CompareTo(elem) < 0) n = n.right;
        else break;
      }
      return n;
    }

    public bool Contains(T elem)
    {
      return this.Find(elem) != this.End;
    }

    /// <summary>
    /// ある値のノードを検索。
    /// （同じ値を持つノード全部を IEnumerable で一覧として返す。）
    /// </summary>
    /// <param name="elem">探したい要素</param>
    /// <returns>見つけたノード一覧</returns>
    public IEnumerable<T> FindRange(T elem)
    {
      for (Node n = this.Find(elem); n != this.End && n.Value.CompareTo(elem) == 0; n = n.Next)
        yield return n.Value;
    }

    #endregion
    #region IEnumerable<T> メンバ

    public IEnumerator<T> GetEnumerator()
    {
      for (Node n = this.Begin; n != this.End; n = n.Next)
        yield return n.Value;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    #endregion
    #region デバッグ用

    [System.Diagnostics.Conditional("DEBUG")]
    public void Output(System.IO.TextWriter writer)
    {
      if (this.root != null)
        this.root.Output(writer, 0);
    }

    #endregion
  }
}
