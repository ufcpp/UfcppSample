using System;
using System.Collections.Generic;
using System.Text;

namespace Collections
{
  /// <summary>
  /// スタック（FILOバッファ）。
  /// </summary>
  /// <typeparam name="T">要素の型</typeparam>
  class Stack<T>
  {
    #region フィールド

    ArrayList<T> buffer;

    #endregion
    #region 初期化

    public Stack() { this.buffer = new ArrayList<T>(); }
    public Stack(int capacity) { this.buffer = new ArrayList<T>(capacity); }

    #endregion
    #region 要素の挿入・削除

    /// <summary>
    /// 要素のプッシュ。
    /// </summary>
    /// <param name="elem">挿入したい要素</param>
    public void Push(T elem)
    {
      this.buffer.InsertLast(elem);
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
      this.buffer.EraseLast();
    }

    /// <summary>
    /// 先頭要素の読み出し。
    /// </summary>
    public T Top
    {
      get { return this.buffer[this.buffer.Count - 1]; }
    }

    public int Count
    {
      get { return this.buffer.Count; }
    }

    #endregion
  }
}
