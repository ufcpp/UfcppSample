using System.Collections.Generic;

namespace Collections
{
  /// <summary>
  /// 循環バッファ。
  /// </summary>
  /// <typeparam name="T">要素の型</typeparam>
  public class CircularBuffer<T> : IEnumerable<T>
  {
    #region フィールド

    T[] data;
    int top, bottom;
    int mask;

    #endregion
    #region 初期化

    public CircularBuffer() : this(256) {}

    /// <summary>
    /// 初期最大容量を指定して初期化。
    /// </summary>
    /// <param name="capacity">初期載大容量</param>
    public CircularBuffer(int capacity)
    {
      capacity = Pow2((uint)capacity);
      this.data = new T[capacity];
      this.top = this.bottom = 0;
      this.mask = capacity - 1;
    }

    static int Pow2(uint n)
    {
      --n;
      int p = 0;
      for (; n != 0; n >>= 1) p = (p << 1) + 1;
      return p + 1;
    }

    #endregion
    #region プロパティ

    /// <summary>
    /// 格納されている要素数。
    /// </summary>
    public int Count
    {
      get
      {
        int count = this.bottom - this.top;
        if (count < 0) count += this.data.Length;
        return count;
      }
    }

    /// <summary>
    /// i 番目の要素を読み書き。
    /// </summary>
    /// <param name="i">読み書き位置</param>
    /// <returns>読み出した要素</returns>
    public T this[int i]
    {
      get { return this.data[(i + this.top) & this.mask]; }
      set { this.data[(i + this.top) & this.mask] = value; }
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
      int i = 0;
      foreach (T elem in this)
      {
        data[i] = elem;
        ++i;
      }
      this.top = 0;
      this.bottom = this.Count;
      this.data = data;
      this.mask = data.Length - 1;
    }

    /// <summary>
    /// i 番目の位置に新しい要素を追加。
    /// </summary>
    /// <param name="i">追加位置</param>
    /// <param name="elem">追加する要素</param>
    public void Insert(int i, T elem)
    {
      if (this.Count >= this.data.Length - 1)
        this.Extend();

      if (i < this.Count / 2)
      {
        for (int n = 0; n <= i; ++n)
        {
          this[n - 1] = this[n];
        }
        this.top = (this.top - 1) & this.mask;
        this[i] = elem;
      }
      else
      {
        for (int n = this.Count; n > i; --n)
        {
          this[n] = this[n - 1];
        }
        this[i] = elem;
        this.bottom = (this.bottom + 1) & this.mask;
      }
    }

    /// <summary>
    /// 先頭に新しい要素を追加。
    /// </summary>
    /// <param name="elem">追加する要素</param>
    public void InsertFirst(T elem)
    {
      if (this.Count >= this.data.Length - 1)
        this.Extend();

      this.top = (this.top - 1) & this.mask;
      this.data[this.top] = elem;
    }

    /// <summary>
    /// 末尾に新しい要素を追加。
    /// </summary>
    /// <param name="elem">追加する要素</param>
    public void InsertLast(T elem)
    {
      if (this.Count >= this.data.Length - 1)
        this.Extend();

      this.data[this.bottom] = elem;
      this.bottom = (this.bottom + 1) & this.mask;
    }

    /// <summary>
    /// i 番目の要素を削除。
    /// </summary>
    /// <param name="i">削除位置</param>
    public void Erase(int i)
    {
      for (int n = i; n < this.Count - 1; ++n)
      {
        this[n] = this[n + 1];
      }
      this.bottom = (this.bottom - 1) & this.mask;
    }

    /// <summary>
    /// 先頭の要素を削除。
    /// </summary>
    public void EraseFirst()
    {
      this.top = (this.top + 1) & this.mask;
    }

    /// <summary>
    /// 末尾の要素を削除。
    /// </summary>
    public void EraseLast()
    {
      this.bottom = (this.bottom - 1) & this.mask;
    }

    #endregion
    #region IEnumerable<T> メンバ

    public IEnumerator<T> GetEnumerator()
    {
      if (this.top <= this.bottom)
      {
        for (int i = this.top; i < this.bottom; ++i)
          yield return this.data[i];
      }
      else
      {
        for (int i = this.top; i < this.data.Length; ++i)
          yield return this.data[i];
        for (int i = 0; i < this.bottom; ++i)
          yield return this.data[i];
      }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    #endregion
  }
}
