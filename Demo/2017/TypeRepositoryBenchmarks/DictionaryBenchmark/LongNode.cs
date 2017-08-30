using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// <see cref="CharNode{T}"/>に対して、
/// string を UTF16 のまま使うんじゃなくて、UTF8 化した上で、8バイトずつパックして ulong の状態で探索を行うバージョン。
///
/// 主たる用途として想定してるのが「プロパティ名と PropertyInfo の辞書」的なものなんだけど、
/// その想定だと、たいていのプロパティは ASCII 8文字以内(つまり、ulong 1個)に収まってたりして、ツリー探索というかほぼ線形探索。
/// </summary>
class LongNode<T> : IReadOnlyDictionary<string, T>, IDictionary<string, T>
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
    List<(ulong key, LongNode<T> node)> _children;

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

    public T this[string key] => TryGetValue(key, out var v) ? v : throw new KeyNotFoundException(key);

    public bool TryGetValue(string key, out T value)
    {
        var n = Find(key, false);
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

    public bool ContainsKey(string key) => TryGetValue(key, out _);

    public void Add(string key, T value)
    {
        var n = Find(key, true);
        n._value = new Opt(value);
    }

    // 8の倍数化
    private static int Align(int x) => ((x + 7) / 8) * 8;

    private LongNode<T> Find(string key, bool add)
    {
        unsafe
        {
            // ここの変換コストがちょっと気になるけども…
            // 実用上は、UTF8 で来た文字列を UTF8 のままキーにして Find したいので、その用途であればこのコストはかからない。
            // string を key にできないのは不便だからお情けで string 版 Find を用意してる感じ。
            var len = Align(key.Length * 3);
            byte* buf = stackalloc byte[len];
            var len2 = StringExtensions.GetBytes(key, new Span<byte>(buf, len));
            var len3 = Align(len2);
            for (int i = len2; i < len3; i++) buf[i] = 0;

            return Find(new Span<byte>(buf, len3).NonPortableCast<byte, ulong>(), 0, add);
        }
    }

    private LongNode<T> Find(Span<ulong> key, int offset, bool add)
    {
        if(key.Length == offset)
        {
            return this;
        }

        if (_children == null) _children = new List<(ulong, LongNode<T>)>();

        var c = key[offset];
        var i = _children.FindIndex(t => t.key == c);
        LongNode<T> n;
        if (i < 0)
        {
            if (add)
            {
                n = new LongNode<T>();
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
