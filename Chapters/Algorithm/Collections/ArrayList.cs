using System.Collections.Generic;

namespace Collections
{
  /// <summary>
  /// 配列リスト。
  /// </summary>
  /// <typeparam name="T">要素の型</typeparam>
  public class ArrayList<T> : IEnumerable<T>
  {
    #region フィールド

    T[] data;
    int count;

    #endregion
    #region 初期化

    public ArrayList() : this(256) {}

    /// <summary>
    /// 初期最大容量を指定して初期化。
    /// </summary>
    /// <param name="capacity">初期最大容量</param>
    public ArrayList(int capacity)
    {
      this.data = new T[capacity];
      this.count = 0;
    }

    #endregion
    #region プロパティ

    /// <summary>
    /// 格納されている要素数。
    /// </summary>
    public int Count
    {
      get { return this.count; }
    }

    /// <summary>
    /// i 番目の要素を読み書き。
    /// </summary>
    /// <param name="i">読み書き位置</param>
    /// <returns>読み出した要素</returns>
    public T this[int i]
    {
      get { return this.data[i]; }
      set { this.data[i] = value; }
    }

    #endregion
    #region 挿入・削除

    /// <summary>
    /// 配列を確保しなおす。
    /// </summary>
    /// <remarks>
    /// 配列長は2倍ずつ拡張していきます。
    /// </remarks>
    void Extend()
    {
      T[] data = new T[this.data.Length * 2];
      for (int i = 0; i < this.data.Length; ++i) data[i] = this.data[i];
      this.data = data;
    }

    /// <summary>
    /// i 番目の位置に新しい要素を追加。
    /// </summary>
    /// <param name="i">追加位置</param>
    /// <param name="elem">追加する要素</param>
    public void Insert(int i, T elem)
    {
      if (this.count >= this.data.Length)
        this.Extend();

      for (int n = this.count; n > i; --n)
      {
        this.data[n] = this.data[n - 1];
      }
      this.data[i] = elem;
      ++this.count;
    }

    /// <summary>
    /// 末尾に新しい要素を追加。
    /// </summary>
    /// <param name="elem">追加する要素</param>
    public void InsertLast(T elem)
    {
      if (this.count >= this.data.Length)
        this.Extend();

      this.data[this.count] = elem;
      ++this.count;
    }

    /// <summary>
    /// i 番目の要素を削除。
    /// </summary>
    /// <param name="i">削除位置</param>
    public void Erase(int i)
    {
      for (int n = i; n < this.count - 1; ++n)
      {
        this.data[n] = this.data[n + 1];
      }
      --this.count;
    }

    /// <summary>
    /// 末尾の要素を削除。
    /// </summary>
    public void EraseLast()
    {
      --this.count;
    }

    #endregion
    #region IEnumerable<T> メンバ

    public IEnumerator<T> GetEnumerator()
    {
      for (int i = 0; i < this.count; ++i)
        yield return this.data[i];
    }

    System.Collections.IEnumerator
      System.Collections.IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    #endregion
  }
}
