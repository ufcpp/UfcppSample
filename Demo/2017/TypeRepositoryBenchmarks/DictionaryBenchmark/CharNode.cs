using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// "aaa", "aa", "abc", "ba", "bb" みたいなのを、
///
/// a---a
///   | +---a
///   +-b---c
/// b---a
///   +-b
///
/// みたいなツリー管理する。
///
/// これ自体からキー検索することもできるけど、そんな処理は重たいことわかってるので、
/// 実際のところこのデータ構造から↓みたいなコードを生成できるといいなぁ、程度のもの。
///
/// <code><![CDATA[
/// if (c[0] == 'a')
/// {
///     if (c[1] == 'a')
///     {
///         if (c.Length == 2) return ...;
///         else if  (c[2] == 'a')
///         {
///             ...
///         }
///     }
/// }
/// ]]></code>
///
/// で、こういう処理をするにしても char のままでやると重たいんで、<see cref="LongNode{T}"/>を使った方がよさげ。
/// </summary>
class CharNode<T> : IReadOnlyDictionary<string, T>, IDictionary<string, T>
{
    struct Opt
    {
        public bool HasValue { get; }
        public T Value { get; }

        public Opt(T value)
        {
            HasValue = true;
            Value = value;
        }
    }

    Opt _value;
    List<(char key, CharNode<T> node)> _children;

    #region not implemented or thin wrapper
    public IEnumerable<string> Keys => throw new NotImplementedException();
    public IEnumerable<T> Values => throw new NotImplementedException();
    public int Count => throw new NotImplementedException();
    ICollection<string> IDictionary<string, T>.Keys => throw new NotImplementedException();
    ICollection<T> IDictionary<string, T>.Values => throw new NotImplementedException();
    public IEnumerator<KeyValuePair<string, T>> GetEnumerator() => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    public bool Remove(string key) => throw new NotImplementedException();
    public void Clear() => throw new NotImplementedException();
    public bool Contains(KeyValuePair<string, T> item) => throw new NotImplementedException();
    public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex) => throw new NotImplementedException();
    public bool Remove(KeyValuePair<string, T> item) => throw new NotImplementedException();
    public bool IsReadOnly => true;
    T IDictionary<string, T>.this[string key] { get => this[key]; set => throw new NotImplementedException(); }
    public void Add(KeyValuePair<string, T> item) => Add(item.Key, item.Value);
    #endregion

    public T this[string key]
    {
        get
        {
            var n = Find(key, 0, false);
            if (n == null || !n._value.HasValue) throw new KeyNotFoundException(key);
            return n._value.Value;
        }
    }

    public bool TryGetValue(string key, out T value)
    {
        var n = Find(key, 0, false);
        if (n == null || !n._value.HasValue)
        {
            value = default;
            return false;
        }
        else
        {
            value = n._value.Value;
            return true;
        }
    }

    public bool ContainsKey(string key)
    {
        var n = Find(key, 0, false);
        return n != null && !n._value.HasValue;
    }

    public void Add(string key, T value)
    {
        var n = Find(key, 0, true);
        n._value = new Opt(value);
    }

    private CharNode<T> Find(string key, int offset, bool add)
    {
        if(key.Length == offset)
        {
            return this;
        }

        if (_children == null) _children = new List<(char, CharNode<T>)>();

        var c = key[offset];
        var i = _children.FindIndex(t => t.key == c);
        CharNode<T> n;
        if (i < 0)
        {
            if (add)
            {
                n = new CharNode<T>();
                _children.Add((c, n));
            }
            else return null;
        }
        else
        {
            n = _children[i].node;
        }

        return n.Find(key, offset + 1, add);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        Append(sb, 0);
        return sb.ToString();
    }

    private void Append(StringBuilder sb, int offset)
    {
        if (_children != null)
        {
            foreach (var (key, node) in _children)
            {
                for (int i = 0; i < offset; i++) sb.Append(' ');
                sb.Append(key);
                sb.AppendLine();

                node.Append(sb, offset + 1);
            }
        }
    }
}
