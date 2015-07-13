using System;
using System.Collections.Generic;
using System.Text;

namespace Collections
{
  /// <summary>
  /// 辞書。
  /// </summary>
  /// <typeparam name="TKey">鍵の型</typeparam>
  /// <typeparam name="TValue">値の型</typeparam>
  public interface IDictionary<TKey, TValue>
    : IEnumerable<KeyValuePair<TKey, TValue>>
  {
    /// <summary>
    /// 新しい要素の挿入。
    /// </summary>
    /// <param name="key">新しい要素の鍵</param>
    /// <param name="elem">新しい要素の値</param>
    void Insert(TKey key, TValue val);

    /// <summary>
    /// 要素の削除。
    /// </summary>
    /// <param name="elem">削除したい要素</param>
    void Erase(TKey key);

    /// <summary>
    /// 要素を含むかどうか。
    /// </summary>
    /// <param name="elem">検索したい要素</param>
    /// <returns>見つかった場合 true</returns>
    bool Contains(TKey key);

    /// <summary>
    /// [] を使って値を取り出す。
    /// </summary>
    /// <param name="key">鍵</param>
    /// <returns>値</returns>
    TValue this[TKey key]
    {
      set;
      get;
    }

    /// <summary>
    /// 鍵一覧取得
    /// </summary>
    IEnumerable<TKey> Keys { get; }

    /// <summary>
    /// 値一覧取得
    /// </summary>
    IEnumerable<TValue> Values { get; }
  }

  /// <summary>
  /// 辞書のエントリー。
  /// </summary>
  /// <typeparam name="TKey">鍵の型</typeparam>
  /// <typeparam name="TValue">値の型</typeparam>
  internal class Entry<TKey, TValue>
  {
    internal TKey key;
    internal TValue val;

    internal Entry(TKey key) : this(key, default(TValue)) { }

    internal Entry(TKey key, TValue val)
    {
      this.key = key;
      this.val = val;
    }

    #region object メンバ

    public override int GetHashCode()
    {
      return this.key.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      Entry<TKey, TValue> ent = obj as Entry<TKey, TValue>;
      if (ent == null) return false;
      return this.key.Equals(ent.key);
    }

    #endregion
  }

  /// <summary>
  /// 辞書のエントリー。
  /// （SortedArray, Tree 用）
  /// 鍵が IComparable を実装している必要あり。
  /// </summary>
  /// <typeparam name="TKey">鍵の型</typeparam>
  /// <typeparam name="TValue">値の型</typeparam>
  internal class ComparableEntry<TKey, TValue>
    : Entry<TKey, TValue>,
    IComparable<ComparableEntry<TKey, TValue>>
    where TKey : IComparable<TKey>
  {
    internal ComparableEntry(TKey key) : base(key) { }
    internal ComparableEntry(TKey key, TValue val) : base(key, val) { }

    #region IComparable<ComparableEntry<TKey,TValue>> メンバ

    public int CompareTo(ComparableEntry<TKey, TValue> other)
    {
      return this.key.CompareTo(other.key);
    }

    #endregion
  }

  public class HashDictionary<TKey, TValue> : IDictionary<TKey, TValue>
  {
    #region フィールド

    HashTable<Entry<TKey, TValue>> table;

    #endregion
    #region 初期化

    public HashDictionary() : this(256) { }

    public HashDictionary(int capacity)
    {
      this.table = new HashTable<Entry<TKey, TValue>>(capacity);
    }

    #endregion
    #region IDictionary<TKey,TValue> メンバ

    public void Insert(TKey key, TValue val)
    {
      this.table.Insert(new Entry<TKey, TValue>(key, val));
    }

    public void Erase(TKey key)
    {
      this.table.Erase(new Entry<TKey, TValue>(key));
    }

    public bool Contains(TKey key)
    {
      return this.table.Contains(new Entry<TKey, TValue>(key));
    }

    public TValue this[TKey key]
    {
      get
      {
        Entry<TKey, TValue> entry = this.table.Find(new Entry<TKey, TValue>(key));
        if (entry == null) return default(TValue);
        return entry.val;
      }
      set
      {
        Entry<TKey, TValue> entry = this.table.Find(new Entry<TKey, TValue>(key));
        if (entry == null) this.Insert(key, value);
        else entry.val = value;
      }
    }

    public IEnumerable<TKey> Keys
    {
      get
      {
        foreach (Entry<TKey, TValue> ent in this.table)
        {
          yield return ent.key;
        }
      }
    }

    public IEnumerable<TValue> Values
    {
      get
      {
        foreach (Entry<TKey, TValue> ent in this.table)
        {
          yield return ent.val;
        }
      }
    }

    #endregion
    #region IEnumerable<T> メンバ

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      foreach (Entry<TKey, TValue> ent in this.table)
      {
        yield return new KeyValuePair<TKey, TValue>(ent.key, ent.val);
      }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    #endregion
  }

  public class SortedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    where TKey : IComparable<TKey>
  {
    #region フィールド

    SortedArray<ComparableEntry<TKey, TValue>> table;

    #endregion
    #region 初期化

    public SortedDictionary() : this(256) { }

    public SortedDictionary(int capacity)
    {
      this.table = new SortedArray<ComparableEntry<TKey, TValue>>(capacity);
    }

    #endregion
    #region IDictionary<TKey,TValue> メンバ

    public void Insert(TKey key, TValue val)
    {
      this.table.Insert(new ComparableEntry<TKey, TValue>(key, val));
    }

    public void Erase(TKey key)
    {
      this.table.Erase(new ComparableEntry<TKey, TValue>(key));
    }

    public bool Contains(TKey key)
    {
      return this.table.Contains(new ComparableEntry<TKey, TValue>(key));
    }

    public TValue this[TKey key]
    {
      get
      {
        ComparableEntry<TKey, TValue> entry = this.table.Find(new ComparableEntry<TKey, TValue>(key));
        if (entry == null) return default(TValue);
        return entry.val;
      }
      set
      {
        ComparableEntry<TKey, TValue> entry = this.table.Find(new ComparableEntry<TKey, TValue>(key));
        if (entry == null) this.Insert(key, value);
        else entry.val = value;
      }
    }

    public IEnumerable<TKey> Keys
    {
      get
      {
        foreach (Entry<TKey, TValue> ent in this.table)
        {
          yield return ent.key;
        }
      }
    }

    public IEnumerable<TValue> Values
    {
      get
      {
        foreach (Entry<TKey, TValue> ent in this.table)
        {
          yield return ent.val;
        }
      }
    }

    #endregion
    #region IEnumerable<T> メンバ

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      foreach (Entry<TKey, TValue> ent in this.table)
      {
        yield return new KeyValuePair<TKey, TValue>(ent.key, ent.val);
      }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    #endregion
  }

  public class TreeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    where TKey : IComparable<TKey>
  {
    #region フィールド

    BinaryTree<ComparableEntry<TKey, TValue>> table;

    #endregion
    #region 初期化

    public TreeDictionary()
    {
      this.table = new BinaryTree<ComparableEntry<TKey, TValue>>();
    }

    #endregion
    #region IDictionary<TKey,TValue> メンバ

    public void Insert(TKey key, TValue val)
    {
      this.table.Insert(new ComparableEntry<TKey, TValue>(key, val));
    }

    public void Erase(TKey key)
    {
      this.table.Erase(new ComparableEntry<TKey, TValue>(key));
    }

    public bool Contains(TKey key)
    {
      return this.table.Contains(new ComparableEntry<TKey, TValue>(key));
    }

    public TValue this[TKey key]
    {
      get
      {
        BinaryTree<ComparableEntry<TKey, TValue>>.Node node
          = this.table.Find(new ComparableEntry<TKey, TValue>(key));
        if (node == null) return default(TValue);
        return node.Value.val;
      }
      set
      {
        BinaryTree<ComparableEntry<TKey, TValue>>.Node node
          = this.table.Find(new ComparableEntry<TKey, TValue>(key));
        if (node == null) this.Insert(key, value);
        else node.Value.val = value;
      }
    }

    public IEnumerable<TKey> Keys
    {
      get
      {
        foreach (Entry<TKey, TValue> ent in this.table)
        {
          yield return ent.key;
        }
      }
    }

    public IEnumerable<TValue> Values
    {
      get
      {
        foreach (Entry<TKey, TValue> ent in this.table)
        {
          yield return ent.val;
        }
      }
    }

    #endregion
    #region IEnumerable<T> メンバ

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      foreach (Entry<TKey, TValue> ent in this.table)
      {
        yield return new KeyValuePair<TKey, TValue>(ent.key, ent.val);
      }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    #endregion
  }
}
