using System;
using System.Collections.Generic;
using System.Text;

namespace Collections
{
  /// <summary>
  /// スタック（FILOバッファ）。
  /// </summary>
  /// <typeparam name="T">要素の型</typeparam>
  class Queue<T>
  {
    #region フィールド

    CircularBuffer<T> buffer;

    #endregion
    #region 初期化

    public Queue() { this.buffer = new CircularBuffer<T>(); }
    public Queue(int capacity) { this.buffer = new CircularBuffer<T>(capacity); }

    #endregion
    #region 要素の挿入・削除

    /// <summary>
    /// 要素のエンキュー。
    /// </summary>
    /// <param name="elem">挿入したい要素</param>
    /// <remarks>
    /// 名前は Stack に合わせて Push にしてある。
    /// </remarks>
    public void Push(T elem)
    {
      this.buffer.InsertLast(elem);
    }

    /// <summary>
    /// 要素を1つデキュー。
    /// </summary>
    /// <remarks>
    /// 名前は Stack に合わせて Pop にしてある。
    /// 今回の実装では、先頭要素の読み出しと削除は別に行う。
    /// この Pop では削除のみ。
    /// 読み出しには Top プロパティを使う。
    /// </remarks>
    public void Pop()
    {
      this.buffer.EraseFirst();
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
